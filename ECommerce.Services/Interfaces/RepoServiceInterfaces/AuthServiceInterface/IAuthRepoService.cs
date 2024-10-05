using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface
{
    public interface IAuthRepoService
    {
        Task<Response<RegisterInputDTO>> RegisterUserAsync(RegisterInputDTO RegisterInputModel);
        Task<Response<JwtTokenDTO>> LoginUserAsync(LoginInpulDTO LoginInputModel);
        Task<Response<User>> FindByUserNameAsync(string? userName);
        Task<Response<User>> FindByEmailAsync(string? email);
    }
}
