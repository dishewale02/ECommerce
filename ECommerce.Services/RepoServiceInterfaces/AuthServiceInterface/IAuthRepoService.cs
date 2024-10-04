using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.RepoServiceInterfaces.AuthRepoServiceInterface
{
    public interface IAuthRepoService
    {
        Task<Response<RegisterInputDTO>> RegisterUserAsync(RegisterInputDTO RegisterInputModel);
        Task<Response<LoginInputModel>> LoginUserAsync(LoginInputModel LoginInputModel);
    }
}
