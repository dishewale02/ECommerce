
using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.AuthRepoInterface;

namespace ECommerce.Services.Classes.RepoServiceClasses.AuthRepoServiceClass
{
    public class Validator
    {
        protected readonly IAuthRepo _authRepo;
        protected readonly IMapper _mapper;

        public Validator(IAuthRepo authRepo, IMapper mapper)
        {
            _authRepo = authRepo;
            _mapper = mapper;
        }   

        public async Task<Response<bool>> ValidateUserAsync(UserInputDTO userInputDTO)
        {
            //check if password is null.
            if (string.IsNullOrWhiteSpace(userInputDTO.Password))
            {
                return Response<bool>.Failure("password can not be null.");
            }

            //check if password and confirm password is matching.
            if (userInputDTO.Password != userInputDTO.ConfirmPassword)
            {
                return Response<bool>.Failure("password and confirm password not matching.");
            }

            return Response<bool>.Success(true);
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

    }
}
