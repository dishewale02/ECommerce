using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface
{
    public interface IAuthRepoService
    {
        Task<Response<RegisterInputDTO>> RegisterUserAsync(RegisterInputDTO RegisterInputModel);
        Task<Response<JwtTokenDTO>> LoginUserAsync(LoginInpulDTO LoginInputModel);
    }
}
