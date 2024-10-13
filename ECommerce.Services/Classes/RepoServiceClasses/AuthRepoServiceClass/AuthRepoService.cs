using AutoMapper;
using ECommerce.Models.DataModels;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.InputModelsDTO.EmailSenderDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.AuthRepoInterface;
using ECommerce.Services.Classes.CryptoService;
using ECommerce.Services.Interfaces.OtherServicesInterfaces.EmailServiceInterface;
using ECommerce.Services.Interfaces.OtherServicesInterfaces.JwtTokenGeneratorInterface;
using ECommerce.Services.Interfaces.OtherServicesInterfaces.PasswordHasherServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface;
using System.Web;

namespace ECommerce.Services.Classes.RepoServiceClasses.AuthRepoServiceClass
{
    public class AuthRepoService : IAuthRepoService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepo _authRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticator _authenticator;
        private readonly AuthenticationConfig _authenticationConfig;
        protected readonly EmailSettings _emailSettings;
        protected readonly IEmailService _emailService;
        protected readonly Validator _validator;

        public AuthRepoService(IMapper mapper, 
            IAuthRepo authRepo, 
            IPasswordHasher passwordHasher, 
            IAuthenticator authenticator, 
            AuthenticationConfig authenticationConfig, 
            EmailSettings emailSettings,
            IEmailService emailService, 
            Validator validator)
        {
            _mapper = mapper;
            _authRepo = authRepo;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
            _authenticationConfig = authenticationConfig;
            _emailSettings = emailSettings;
            _emailService = emailService;
            _validator = validator;
        }

        public async Task<Response<JwtTokenDTO>> LoginUserAsync(LoginInpulDTO loginInputModel)
        {
            try
            {
                //find if user is null or not.
                if (loginInputModel is null || loginInputModel.Password is null || loginInputModel.UserName is null)
                {
                    return Response<JwtTokenDTO>.Failure("provided input is not valid.");
                }

                //validate username in database by both username or email perameter.
                Response<User> foundUserResponse = await _authRepo.FindByUserNameAsync(loginInputModel.UserName);

                if (!foundUserResponse.IsSuccessfull)
                {
                    foundUserResponse = await _authRepo.FindByEmailAsync(loginInputModel.UserName);

                    if (!foundUserResponse.IsSuccessfull)
                    {
                        return Response<JwtTokenDTO>.Failure(foundUserResponse.ErrorMessage);
                    }
                }
                
                //validate password.
                Response<bool> verifyPasswordInDatabaseResponse = await _passwordHasher.VerifyPasswordAsync(loginInputModel.Password, foundUserResponse.Value.PasswordHash);

                if (!verifyPasswordInDatabaseResponse.IsSuccessfull)
                {
                    return Response<JwtTokenDTO>.Failure(verifyPasswordInDatabaseResponse.ErrorMessage);
                }

                //create and save token
                Response<JwtTokenDTO> tokenCreationAndSaveResponse = await CreateAndSaveTokenAsync(foundUserResponse.Value);

                if (!tokenCreationAndSaveResponse.IsSuccessfull)
                {
                    return Response<JwtTokenDTO>.Failure(tokenCreationAndSaveResponse.ErrorMessage);
                }

                return Response<JwtTokenDTO>.Success(tokenCreationAndSaveResponse.Value);

            }
            catch (Exception ex)
            {
                return Response<JwtTokenDTO>.Failure(ex.Message);
            }
        }

        public async Task<Response<UserInputDTO>> RegisterUserAsync(UserInputDTO registerInputModel)
        {
            //check if input model is empty.
            if (registerInputModel == null)
            {
                return Response<UserInputDTO>.Failure("input can not empty");
            }
            else
            {
                try
                {
                    //send input userModel to validator.
                    Response<bool> inputUserValidatorResponse = await _validator.ValidateUserAsync(registerInputModel);

                    //check response.
                    if(!inputUserValidatorResponse.IsSuccessfull)
                    {
                        return Response<UserInputDTO>.Failure(inputUserValidatorResponse.ErrorMessage);
                    }

                    //create new UserInputDTO instance.
                    Response<User> foundUserResponse = new Response<User>();

                    //check if User already available by UserName.
                    Response<User> foundByUserName = await _validator.FindByUserNameAsync(registerInputModel.UserName);

                    //check if user already available by Email.
                    Response<User> foundByEmail = await _validator.FindByEmailAsync(registerInputModel.Email);

                    //check response.
                    if (foundByEmail.IsSuccessfull || foundByUserName.IsSuccessfull)
                    {
                        return Response<UserInputDTO>.Failure("user already available.");
                    }

                    if (foundByUserName.IsSuccessfull)
                    {
                        foundUserResponse = foundByUserName;
                    }
                    else
                    {
                        foundUserResponse = foundByEmail;
                    }

                    //map registerInputModel to User
                    User convertToUser = _mapper.Map<User>(registerInputModel);

                    //send result to password hasher.
                    Response<string> passwordHashResponse = await _passwordHasher.GenerateHashAsync(registerInputModel.Password);

                    //check password hasher response.
                    if (!passwordHashResponse.IsSuccessfull)
                    {
                        return Response<UserInputDTO>.Failure(passwordHashResponse.ErrorMessage);
                    }

                    //fill other informations.
                    convertToUser.Id = Guid.NewGuid().ToString();
                    convertToUser.CreatedOn = DateTime.Now;
                    convertToUser.PasswordHash = passwordHashResponse.Value;
                    convertToUser.Role = "USER";

                    //send model to Repository layer.
                    Response<User> registerUserResponse = await _authRepo.CreateUserAsync(convertToUser);

                    //check register response.
                    if (!registerUserResponse.IsSuccessfull)
                    {
                        return Response<UserInputDTO>.Failure(registerUserResponse.ErrorMessage);
                    }

                    //map User to UserInputDTO.
                    UserInputDTO convertedToRegisterInputModel = _mapper.Map<UserInputDTO>(registerUserResponse.Value);

                    return Response<UserInputDTO>.Success(convertedToRegisterInputModel);
                }
                catch (Exception ex)
                {
                    return Response<UserInputDTO>.Failure(ex.Message);
                }
            }
        }

        public async Task<Response<JwtTokenDTO>> CreateAndSaveTokenAsync(User user)
        {
            //check if user is null or not.
            if(user.Id == null || user == null)
            {
                return Response<JwtTokenDTO>.Failure("User not available.");
            }

            //generate new access and refresh tokens.
            Response<JwtTokenDTO> tokenGenerateResponse = await _authenticator.GenerateJwtTokensAsync(user);

            if(!tokenGenerateResponse.IsSuccessfull)
            {
                return Response<JwtTokenDTO>.Failure(tokenGenerateResponse.ErrorMessage);
            }

            //map JwtTokenDTO to JwtToken.
            JwtToken jwtTokenDTOToJwtToken = _mapper.Map<JwtToken>(tokenGenerateResponse.Value);

            //Fill other information in Token instance.
            jwtTokenDTOToJwtToken.Id = Guid.NewGuid().ToString();
            jwtTokenDTOToJwtToken.User = user;
            jwtTokenDTOToJwtToken.UserId = user.Id;
            jwtTokenDTOToJwtToken.IsDeleted = false;
            jwtTokenDTOToJwtToken.IsActive = true;
            jwtTokenDTOToJwtToken.CreatedBy = null;
            jwtTokenDTOToJwtToken.ModifiedBy = null;
            jwtTokenDTOToJwtToken.CreatedOn = DateTime.UtcNow;
            jwtTokenDTOToJwtToken.ModifiedOn = DateTime.UtcNow;
            jwtTokenDTOToJwtToken.RefreshTokenValidityTill = DateTime.UtcNow.AddMinutes(_authenticationConfig.RefreshTokenExpirationMinutes);

            //save tokens in database.
            Response<JwtToken> saveTokenInDatabaseResponse = await _authRepo.SaveTokenInDatabaseAsync(jwtTokenDTOToJwtToken);

            if(!saveTokenInDatabaseResponse.IsSuccessfull)
            {
                return Response<JwtTokenDTO>.Failure(saveTokenInDatabaseResponse.ErrorMessage);
            }

            return Response<JwtTokenDTO>.Success(tokenGenerateResponse.Value);
        }

        public async Task<Response<UpdateUserOutputModelDTO>> UpdateUserAsync(UpdateUserInputModelDTO UpdateUserInputModelDTO)
        {
            //check input User information is null 
            if(UpdateUserInputModelDTO == null || UpdateUserInputModelDTO.UserName == null)
            {
                return Response<UpdateUserOutputModelDTO>.Failure("input User details can not be blank.");
            }

            //check if the user is already available in database or not.
            Response<User> foundUserInDatabase = await _validator.FindByUserNameAsync(UpdateUserInputModelDTO.UserName);

            if(!foundUserInDatabase.IsSuccessfull)
            {
                return Response<UpdateUserOutputModelDTO>.Failure(foundUserInDatabase.ErrorMessage);
            }

            //map the UpdateUserInputModelDTO to User.
            User mappedUser = _mapper.Map<User>(UpdateUserInputModelDTO);

            //find User in database.
            Response<User> findUserResponse = await _authRepo.FindUserAsync(UpdateUserInputModelDTO.UserName);

            if (!findUserResponse.IsSuccessfull || findUserResponse.Value.Id == null)
            {
                return Response<UpdateUserOutputModelDTO>.Failure(findUserResponse.ErrorMessage);
            }

            //send Updated user model to update data in database.
            Response<User> updatedUserResponse = await _authRepo.UpdateUserDataAsync(findUserResponse.Value.Id, mappedUser);

            if(!updatedUserResponse.IsSuccessfull)
            {
                return Response<UpdateUserOutputModelDTO>.Failure(updatedUserResponse.ErrorMessage);
            }

            //mapp User to UpdatedUserOutputModel.
            UpdateUserOutputModelDTO updateUserOutputModelDTO = _mapper.Map<UpdateUserOutputModelDTO>(updatedUserResponse.Value);

            return Response<UpdateUserOutputModelDTO>.Success(updateUserOutputModelDTO);
        }

        public async Task<Response<bool>> ResetPasswordAsync(ResetPasswordInputDTO model)
        {
            try
            {
                string decryptedData = AESEncryptDecryptService.Decrypt(model.Token);

                string[] splittedData = decryptedData.Split('|');

                string email = splittedData[0];

                Response<User> foundUserResponse = await _validator.FindByEmailAsync(email);
                if (!foundUserResponse.IsSuccessfull)
                {
                    return Response<bool>.Failure("token is invalid.");
                }

                //do password hash for input model new password.
                Response<string> passwordHashResponse = await _passwordHasher.GenerateHashAsync(model.NewPassword);

                //update the found user password field to new password field of ResetRasswordModel.
                foundUserResponse.Value.PasswordHash = passwordHashResponse.Value;

                //map UserOutputDTO to UpdateUserInputModelDTO
                UpdateUserInputModelDTO updateUserOutputModelDTO = _mapper.Map<UpdateUserInputModelDTO>(foundUserResponse.Value);

                //update user in database.
                Response<UpdateUserOutputModelDTO> updatedUserResponse = await UpdateUserAsync(updateUserOutputModelDTO);

                if (!updatedUserResponse.IsSuccessfull)
                {
                    return Response<bool>.Failure(updatedUserResponse.ErrorMessage);
                }

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex.Message);
            }
        }

        public async Task<Response<string>> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            try
            {
                //send email id to verify availablity in database.
                Response<User> foundUserResponse = await _validator.FindByEmailAsync(model.Email);
                if (!foundUserResponse.IsSuccessfull)
                {
                    return Response<string>.Failure("User does not exist or email not confirmed.");
                }

                User availableUser = foundUserResponse.Value;
                
                //update User in database with reset token.
                string expirationDateTimeString = DateTime.Now.AddMinutes(60).ToString(); //valid for 1hr

                string dataToEncrypt = $"{availableUser.Email}|{expirationDateTimeString}";

                string encryptedToken = AESEncryptDecryptService.Encrypt(dataToEncrypt);

                string urlEncodedToken = HttpUtility.UrlEncode(encryptedToken);

                //generate reset password link.
                string resetLink = $"{_emailSettings.ReturnRequestServer}/reset-password?token={urlEncodedToken}";

                //send email with reset link.
                await _emailService.SendEmailAsync(availableUser.Email!, "Reset Password", $"Click <a href='{resetLink}'>here</a> to reset your password.");

                return Response<string>.Success("Password reset link has been sent to your email.");
            }
            catch (Exception ex)
            {
                return Response<string>.Failure(ex.Message);
            }
        }

        public async Task<Response<bool>> CheckForgotPasswordTokenAsync(CheckForgotPasswordTokenModel model)
        {
            try
            {
                string decryptedData = AESEncryptDecryptService.Decrypt(model.Token);

                string[] splittedData = decryptedData.Split('|');

                string email = splittedData[0];

                Response<User> foundUserResponse = await _validator.FindByEmailAsync(email);
                if (!foundUserResponse.IsSuccessfull)
                {
                    return Response<bool>.Failure("Token is invalid");
                }

                string expirationDateTimeString = splittedData[1];
                DateTime expirationDateTime;

                if (!DateTime.TryParse(expirationDateTimeString, out expirationDateTime))
                {
                    return Response<bool>.Failure("Token is invalid");
                }

                if (DateTime.Now > expirationDateTime)
                {
                    return Response<bool>.Failure("Link is expired.");
                }

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex.Message);
            }
        }
    }
}
