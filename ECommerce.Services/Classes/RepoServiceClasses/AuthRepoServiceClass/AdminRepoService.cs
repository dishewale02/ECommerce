using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.DataModels.InfoModel;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Classes.GenericRepoClass;
using ECommerce.Repo.Interfaces.AdminRepoInterface;
using ECommerce.Repo.Interfaces.GenericRepoInterface;
using ECommerce.Services.Classes.RepoServiceClasses.GenericRepoServiceClass;
using ECommerce.Services.Interfaces.OtherServicesInterfaces.PasswordHasherServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface;

namespace ECommerce.Services.Classes.RepoServiceClasses.AuthRepoServiceClass
{
    public class AdminRepoService : GenericRepoService<UserInputDTO, User>, IAdminRepoService
    {
        protected readonly Validator _validator;
        protected readonly IPasswordHasher _passwordHasher;
        private readonly IAdminRepo _adminRepo;

        public AdminRepoService(IGenericRepo<User> genericRepo, IMapper mapper, Validator validator, IPasswordHasher passwordHasher, IAdminRepo adminRepo) : base(genericRepo, mapper)
        {
            _validator = validator;
            _passwordHasher = passwordHasher;
            _adminRepo = adminRepo;
        }

        public override async Task<Response<UserInputDTO>> CreateAsync(UserInputDTO userInputDTO, UserClaimModel userClaimModel)
        {
            try
            {
                //check if input entity is null.
                if (userInputDTO is null || userClaimModel is null)
                {
                    return Response<UserInputDTO>.Failure("entity can not be null.");
                }

                //send input userModel to validator.
                Response<bool> inputUserValidatorResponse = await _validator.ValidateUserAsync(userInputDTO);

                //check response.
                if (!inputUserValidatorResponse.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure(inputUserValidatorResponse.ErrorMessage);
                }

                //create new UserInputDTO instance.
                Response<User> foundUserResponse = new Response<User>();

                //check if User already available by UserName.
                foundUserResponse = await _validator.FindByUserNameAsync(userInputDTO.UserName);

                //check response.
                if (foundUserResponse.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure("user already available.");
                }

                //check if user already available by Email.
                foundUserResponse = await _validator.FindByEmailAsync(userInputDTO.Email);

                //check response.
                if (foundUserResponse.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure("user already available.");
                }

                //send result to password hasher.
                Response<string> passwordHashResponse = await _passwordHasher.GenerateHashAsync(userInputDTO.Password);

                //check password hasher response.
                if (!passwordHashResponse.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure(passwordHashResponse.ErrorMessage);
                }

                //convert Input model to User.
                User convertToUser = _mapper.Map<User>(userInputDTO);

                //fill other informations.
                convertToUser.Id = Guid.NewGuid().ToString();
                convertToUser.CreatedBy = userClaimModel.UserName;
                convertToUser.CreatedOn = DateTime.Now;
                convertToUser.PasswordHash = passwordHashResponse.Value;
                convertToUser.IsDeleted = false;
                convertToUser.IsActive = true;
                convertToUser.Role = "USER";

                //send entity to save in database.
                Response<User> saveUserInDatabaseResponse = await _genericRepo.RCreateAsync(convertToUser);

                //mapp response entity to TTarget model.

                UserInputDTO mappedUserToUserInputDTO = _mapper.Map<User, UserInputDTO>(saveUserInDatabaseResponse.Value);

                mappedUserToUserInputDTO.Password = null;
                mappedUserToUserInputDTO.ConfirmPassword = null;
                mappedUserToUserInputDTO.PasswordHash = null;

                //check if response is successfull.
                if (!saveUserInDatabaseResponse.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure(saveUserInDatabaseResponse.ErrorMessage);
                }

                return Response<UserInputDTO>.Success(mappedUserToUserInputDTO);
            }
            catch (Exception ex)
            {
                return Response<UserInputDTO>.Failure(ex.Message);
            }
        }

        public async Task<Response<UserInputDTO>> UpdateAsync(UserInputDTO updateUserInput, UserClaimModel userClaimModel)
        {
            try
            {
                //check if input entity is null.
                if (updateUserInput is null || userClaimModel is null)
                {
                    return Response<UserInputDTO>.Failure("entity can not be null.");
                }

                //send input userModel to validator.
                Response<bool> inputUserValidatorResponse = await _validator.ValidateUserAsync(updateUserInput);

                //check response.
                if (!inputUserValidatorResponse.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure(inputUserValidatorResponse.ErrorMessage);
                }

                //create new UserInputDTO instance.
                Response<User> foundUserResponse = new Response<User>();

                //check if User already available by UserName.
                Response<User> foundByUserName = await _validator.FindByUserNameAsync(updateUserInput.UserName);

                //check if user already available by Email.
                Response<User> foundByEmail = await _validator.FindByEmailAsync(updateUserInput.Email);

                //check response.
                if (!foundByEmail.IsSuccessfull && !foundByUserName.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure(foundByEmail.ErrorMessage);
                }

                if(foundByUserName.IsSuccessfull)
                {
                    foundUserResponse = foundByUserName;
                }
                else
                {
                    foundUserResponse = foundByEmail;
                }

                //get password hashed from the hasher.
                Response<string> passwordHashResponse = await _passwordHasher.GenerateHashAsync(updateUserInput.Password);

                //check response.
                if (!passwordHashResponse.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure(passwordHashResponse.ErrorMessage);
                }

                //save passwordhash into the inputDTO
                updateUserInput.PasswordHash = passwordHashResponse.Value;

                //mapp the UserInputDTO to User model.
                User mappedUserInputDTOToUser = _mapper.Map<User>(foundUserResponse.Value);

                //update required fields from the mappedUserModel.
                mappedUserInputDTOToUser.UserName = updateUserInput.UserName;
                mappedUserInputDTOToUser.FirstName = updateUserInput.FirstName;
                mappedUserInputDTOToUser.LastName = updateUserInput.LastName;
                mappedUserInputDTOToUser.Email = updateUserInput.Email;
                mappedUserInputDTOToUser.PasswordHash = updateUserInput.PasswordHash;
                mappedUserInputDTOToUser.Phone = updateUserInput.Phone;
                mappedUserInputDTOToUser.Role = updateUserInput.Role;
                mappedUserInputDTOToUser.Id = foundUserResponse.Value.Id;
                mappedUserInputDTOToUser.ModifiedBy = userClaimModel.UserName;
                mappedUserInputDTOToUser.ModifiedOn = DateTime.UtcNow;
                mappedUserInputDTOToUser.CreatedBy = foundUserResponse.Value.CreatedBy;
                mappedUserInputDTOToUser.CreatedOn = foundUserResponse.Value.CreatedOn;

                //send the updated model to the database.
                Response<User> updatedResponse = await _genericRepo.RUpdateAsync(mappedUserInputDTOToUser);

                //mapp User to UserInputDTO.
                UserInputDTO mappedUserToUserInputDTO = _mapper.Map<UserInputDTO>(updatedResponse.Value);


                mappedUserToUserInputDTO.Password = null;
                mappedUserToUserInputDTO.ConfirmPassword = null;
                mappedUserToUserInputDTO.PasswordHash = null;

                //check response.
                if (!updatedResponse.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure(updatedResponse.ErrorMessage);
                }

                return Response<UserInputDTO>.Success(mappedUserToUserInputDTO);
            }
            catch (Exception ex)
            {
                return Response<UserInputDTO>.Failure(ex.Message);
            }
        }

        public async Task<Response<List<UserInputDTO>>> GetAllDeletedAndNonActiveUsers()
        {
            try
            {
                //send request to data layer.
                Response<List<User>> deletedAndNonActiveUsersResponse = await _adminRepo.RGetDeletedAndNonActiveUsers();

                //check data layer response
                if (!deletedAndNonActiveUsersResponse.IsSuccessfull)
                {
                    return Response<List<UserInputDTO>>.Failure(deletedAndNonActiveUsersResponse.ErrorMessage);
                }

                //create new response list.
                List<UserInputDTO> userResponse = new List<UserInputDTO>();

                //map and save entry in new list.
                foreach (User user in deletedAndNonActiveUsersResponse.Value)
                {
                    UserInputDTO mappedUser = _mapper.Map<UserInputDTO>(user);

                    userResponse.Add(mappedUser);
                }

                //check response List.
                if (userResponse.Count == 0)
                {
                    return Response<List<UserInputDTO>>.Failure("internal mapping error.");
                }

                return Response<List<UserInputDTO>>.Success(userResponse);
            }
            catch (Exception ex)
            {
                return Response<List<UserInputDTO>>.Failure(ex.Message);
            }
        }

        public async Task<Response<UserInputDTO>> ActivateDeletedUserAsync(string userId)
        {
            try
            {
                //check if input id is null.
                if (string.IsNullOrEmpty(userId))
                {
                    return Response<UserInputDTO>.Failure("input id is null.");
                }

                //send request to delete entity.
                Response<User> activateDeletedUserResponse = await _adminRepo.RActivateUserAsync(userId);

                //check Response to return
                if (!activateDeletedUserResponse.IsSuccessfull)
                {
                    return Response<UserInputDTO>.Failure(activateDeletedUserResponse.ErrorMessage);
                }

                //mapp deleteEntityModel to TTarget model.
                UserInputDTO TSourceToTTargetMapped = _mapper.Map<User, UserInputDTO>(activateDeletedUserResponse.Value);

                if (TSourceToTTargetMapped == null)
                {
                    return Response<UserInputDTO>.Failure("internal mapping error.");
                }

                return Response<UserInputDTO>.Success(TSourceToTTargetMapped);
            }
            catch (Exception ex)
            {
                return Response<UserInputDTO>.Failure(ex.Message);
            }
        }
    }
}
