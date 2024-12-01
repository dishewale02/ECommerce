using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface
{
    public interface IAdminRepoService
    {
        Task<Response<UserInputDTO>> CreateAsync(UserInputDTO userInputDTO, UserClaimModel userClaimModel);
        Task<Response<UserInputDTO>> UpdateAsync(UserInputDTO updateUserInput, UserClaimModel userClaimModel);
        Task<Response<List<UserInputDTO>>> GetAllDeletedAndNonActiveUsers();
        Task<Response<UserInputDTO>> ActivateDeletedUserAsync(string userId);
    }
}