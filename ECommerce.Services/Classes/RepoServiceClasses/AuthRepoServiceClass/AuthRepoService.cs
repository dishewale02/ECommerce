using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.AuthRepoInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.JwtTokenGeneratorInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.PasswordHasherServiceInterface;

namespace ECommerce.Services.Classes.RepoServiceClasses.AuthRepoServiceClass
{
    public class AuthRepoService : IAuthRepoService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepo _authRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticator _authenticator;

        public AuthRepoService(IMapper mapper, IAuthRepo authRepo, IPasswordHasher passwordHasher, IAuthenticator authenticator)
        {
            _mapper = mapper;
            _authRepo = authRepo;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
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
                Response<JwtTokenDTO> tokenResult = await CreateAndSaveTokenAsync(foundUserResponse.Value);

                if (!tokenResult.IsSuccessfull)
                {
                    return Response<JwtTokenDTO>.Failure(tokenResult.ErrorMessage);
                }

                return Response<JwtTokenDTO>.Success(tokenResult.Value);

            }
            catch (Exception ex)
            {
                return Response<JwtTokenDTO>.Failure(ex.Message);
            }
        }

        public async Task<Response<RegisterInputDTO>> RegisterUserAsync(RegisterInputDTO registerInputModel)
        {
            //check if input model is empty.
            if (registerInputModel == null)
            {
                return Response<RegisterInputDTO>.Failure("input can not empty");
            }
            else
            {
                try
                {
                    //check if password is null.
                    if (string.IsNullOrWhiteSpace(registerInputModel.Password))
                    {
                        return Response<RegisterInputDTO>.Failure("password can not be null.");
                    }

                    //check if password and confirm password is matching.
                    if (registerInputModel.Password != registerInputModel.ConfirmPassword)
                    {
                        return Response<RegisterInputDTO>.Failure("password and confirm password not matching.");
                    }

                    //check if User already available by UserName.
                    Response<User> foundByUserNameResponse = await FindByUserNameAsync(registerInputModel.UserName);

                    //check response.
                    if (foundByUserNameResponse.IsSuccessfull)
                    {
                        return Response<RegisterInputDTO>.Failure("user already registered.");
                    }

                    //check if user already available by Email.
                    Response<User> findByEmailResponse = await FindByEmailAsync(registerInputModel.Email);

                    //check response.
                    if (findByEmailResponse.IsSuccessfull)
                    {
                        return Response<RegisterInputDTO>.Failure("user already registered.");
                    }

                    //map registerInputModel to registerModelDTO
                    User convertToUser = _mapper.Map<User>(registerInputModel);

                    //send result to password hasher.
                    Response<string> passwordHashResponse = await _passwordHasher.GenerateHashAsync(registerInputModel.Password);

                    //check password hasher response.
                    if (!passwordHashResponse.IsSuccessfull)
                    {
                        return Response<RegisterInputDTO>.Failure(passwordHashResponse.ErrorMessage);
                    }

                    //fill other informations.
                    convertToUser.CreatedOn = DateTime.Now;
                    convertToUser.PasswordHash = passwordHashResponse.Value;
                    convertToUser.ResetToken = null;
                    convertToUser.ResetTokenExpiry = null;

                    //send model to Repository layer.
                    Response<User> registerUserResponse = await _authRepo.CreateUserAsync(convertToUser);

                    //check register response.
                    if (!registerUserResponse.IsSuccessfull)
                    {
                        return Response<RegisterInputDTO>.Failure(registerUserResponse.ErrorMessage);
                    }

                    //map register model to Register Input Model.
                    RegisterInputDTO convertedToRegisterInputModel = _mapper.Map<RegisterInputDTO>(registerUserResponse.Value);

                    return Response<RegisterInputDTO>.Success(convertedToRegisterInputModel);
                }
                catch (Exception ex)
                {
                    return Response<RegisterInputDTO>.Failure(ex.Message);
                }
            }
        }

        public async Task<Response<User>> FindByUserNameAsync(string? userName)
        {
            //check if input is null.
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Response<User>.Failure("input email string is null.");
            }
            else
            {
                //get find user by UserName.
                Response<User>? foundUserByUserNameResponse = await _authRepo.FindByUserNameAsync(userName);

                //check response.
                if (!foundUserByUserNameResponse.IsSuccessfull)
                {
                    return Response<User>.Failure(foundUserByUserNameResponse.ErrorMessage);
                }
                else
                {
                    return Response<User>.Success(foundUserByUserNameResponse.Value);
                }
            }
        }

        public async Task<Response<User>> FindByEmailAsync(string? email)
        {
            //check if input is null.
            if (string.IsNullOrWhiteSpace(email))
            {
                return Response<User>.Failure("input email string is null.");
            }
            else
            {
                //get find user by Email id.
                Response<User> foundUserByEmailResponse = await _authRepo.FindByEmailAsync(email);

                //check response.
                if (!foundUserByEmailResponse.IsSuccessfull)
                {
                    return Response<User>.Failure(foundUserByEmailResponse.ErrorMessage);
                }
                else
                {
                    return Response<User>.Success(foundUserByEmailResponse.Value);
                }
            }
        }

        public async Task<Response<JwtTokenDTO>> CreateAndSaveTokenAsync(User user)
        {
            //generate new access and refresh tokens.
            Response<JwtTokenDTO> tokenGenerateResponse = await _authenticator.GenerateJwtTokensAsync(user);

            if(!tokenGenerateResponse.IsSuccessfull)
            {
                return Response<JwtTokenDTO>.Failure(tokenGenerateResponse.ErrorMessage);
            }

            return Response<JwtTokenDTO>.Success(tokenGenerateResponse.Value);
            //save tokens in database.
        }

    }
}
