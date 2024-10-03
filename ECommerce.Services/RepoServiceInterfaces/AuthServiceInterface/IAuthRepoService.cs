using ECommerce.Models.InputModels.AuthInputModels;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.RepoServiceInterfaces.AuthRepoServiceInterface
{
    public interface IAuthRepoService
    {
        Task<Response<RegisterInputModel>> RegisterUserAsync(RegisterInputModel RegisterInputModel);
        Task<Response<LoginInputModel>> LoginUserAsync(LoginInputModel LoginInputModel);
    }
}
