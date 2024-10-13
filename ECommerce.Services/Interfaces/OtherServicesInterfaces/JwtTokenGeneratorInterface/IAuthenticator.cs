using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.Interfaces.OtherServicesInterfaces.JwtTokenGeneratorInterface
{
    public interface IAuthenticator
    {
        Task<Response<JwtTokenDTO>> GenerateJwtTokensAsync(User user);
    }
}
