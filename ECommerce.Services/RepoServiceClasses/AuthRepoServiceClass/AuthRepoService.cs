using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.AuthRepoInterface;
using ECommerce.Services.RepoServiceInterfaces.AuthRepoServiceInterface;
using ShoppingCart.Services.HasherService;

namespace ECommerce.Services.RepoServiceClasses.AuthRepoServiceClass
{
    public class AuthRepoService : IAuthRepoService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepo _authRepo;
        private readonly IPasswordHasher _passwordHasher;

        public AuthRepoService(IMapper mapper, IAuthRepo authRepo, IPasswordHasher passwordHasher)
        {
            _mapper = mapper;
            _authRepo = authRepo;
            _passwordHasher = passwordHasher;
        }

        public Task<Response<LoginInputModel>> LoginUserAsync(LoginInputModel loginInputModel)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<RegisterInputDTO>> RegisterUserAsync(RegisterInputDTO registerInputModel)
        {
            //check if input model is empty.
            if(registerInputModel == null)
            {
                return new Response<RegisterInputDTO>("input can not empty");
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
                    if(foundByUserNameResponse.IsSuccessfull)
                    {
                        return Response<RegisterInputDTO>.Failure("User Name is already Available.");
                    }

                    //check if user already available by Email.
                    Response<User> findByEmailResponse = await FindByEmailAsync(registerInputModel.Email);

                    //check response.
                    if(findByEmailResponse.IsSuccessfull)
                    {
                        return Response<RegisterInputDTO>.Failure("Email id is already available.");
                    }

                    //map registerInputModel to registerModelDTO
                    User convertToUser = _mapper.Map<User>(registerInputModel);

                    //send result to password hasher.
                    Response<string> passwordHashResponse = await _passwordHasher.GenerateHashAsync(registerInputModel.Password);

                    //check password hasher response.
                    if(!passwordHashResponse.IsSuccessfull)
                    {
                        return Response<RegisterInputDTO>.Failure(passwordHashResponse.ErrorMessage);
                    }

                    //fill other informations.
                    convertToUser.CreatedOn = DateTime.Now;
                    convertToUser.PasswordHash = passwordHashResponse.Value;

                    //send model to Repository layer.
                    Response<User> registerUserResponse = await _authRepo.CreateUserAsync(convertToUser);

                    //check register response.
                    if(!registerUserResponse.IsSuccessfull)
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

        private async Task<Response<User>> FindByUserNameAsync(string? userName)
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

        private async Task<Response<User>> FindByEmailAsync(string? email)
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

    }
}
