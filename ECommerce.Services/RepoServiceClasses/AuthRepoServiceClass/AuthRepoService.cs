using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.DTOsModels.AuthDTOsModels;
using ECommerce.Models.InputModels.AuthInputModels;
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

        public async Task<Response<RegisterInputModel>> RegisterUserAsync(RegisterInputModel registerInputModel)
        {
            //check if input model is empty.
            if(registerInputModel == null)
            {
                return new Response<RegisterInputModel>("input can not empty");
            }
            else
            {
                try
                {
                    //check if password is null.
                    if (registerInputModel.Password is null)
                    {
                        return new Response<RegisterInputModel>("password can not be null.");
                    }

                    //check if password and confirm password is matching.
                    if (registerInputModel.Password != registerInputModel.ConfirmPassword)
                    {
                        return new Response<RegisterInputModel>("password and confirm password not matching.");
                    }

                    //check if User already available by UserName.
                    Response<User> foundByUserNameResponse = await FindByUserNameAsync(registerInputModel.UserName);

                    //check response.
                    if(foundByUserNameResponse.Value is not null)
                    {
                        return new Response<RegisterInputModel>("User Name is already Available.");
                    }

                    //check if user already available by Email.
                    Response<User> findByEmailResponse = await FindByEmailAsync(registerInputModel.Email);

                    //check response.
                    if(!findByEmailResponse.IsSuccessfull)
                    {
                        return new Response<RegisterInputModel>("Email id is already available.");
                    }

                    //map registerInputModel to registerModelDTO
                    User convertToUser = _mapper.Map<User>(registerInputModel);

                    //send result to password hasher.
                    Response<string> passwordHashResponse = await _passwordHasher.GenerateHashAsync(registerInputModel.Password);

                    //check password hasher response.
                    if(!passwordHashResponse.IsSuccessfull)
                    {
                        return new Response<RegisterInputModel>(passwordHashResponse.ErrorMessage);
                    }

                    //fill other informations.
                    convertToUser.CreatedOn = DateTime.Now;
                    convertToUser.Password = passwordHashResponse.Value;

                    //send model to Repository layer.
                    Response<User> registerUserResponse = await _authRepo.RegisterAsync(convertToUser);

                    //check register response.
                    if(!registerUserResponse.IsSuccessfull)
                    {
                        return new Response<RegisterInputModel>(registerUserResponse.ErrorMessage);
                    }

                    //map register model to Register Input Model.
                    RegisterInputModel convertedToRegisterInputModel = _mapper.Map<RegisterInputModel>(registerUserResponse.Value);

                    return new Response<RegisterInputModel>(convertedToRegisterInputModel);
                }
                catch (Exception ex)
                {
                    return new Response<RegisterInputModel>(ex.Message);
                }
            }
        }

        private async Task<Response<User>> FindByUserNameAsync(string? userName)
        {
            //check if input is null.
            if (userName == null)
            {
                return new Response<User>("input email string is null.");
            }
            else
            {
                //get find user by UserName.
                Response<User> foundUserByUserNameResponse = await _authRepo.FindByUserNameAsync(userName);

                //check response.
                if (foundUserByUserNameResponse.IsSuccessfull)
                {
                    return new Response<User>(foundUserByUserNameResponse.ErrorMessage);
                }
                else
                {
                    return new Response<User>(foundUserByUserNameResponse.Value);
                }
            }
        }

        private async Task<Response<User>> FindByEmailAsync(string? email)
        {
            //check if input is null.
            if (email == null)
            {
                return new Response<User>("input email string is null.");
            }
            else
            {
                //get find user by Email id.
                Response<User> foundUserByEmailResponse = await _authRepo.FindByEmailAsync(email);

                //check response.
                if (foundUserByEmailResponse.Value is null)
                {
                    return new Response<User>(foundUserByEmailResponse.ErrorMessage);
                }
                else
                {
                    return new Response<User>(foundUserByEmailResponse.Value);
                }
            }
        }

    }
}
