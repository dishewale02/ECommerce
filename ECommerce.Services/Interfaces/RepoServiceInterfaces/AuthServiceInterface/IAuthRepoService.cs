using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface
{
    public interface IAuthRepoService
    {
        Task<Response<UserInputDTO>> RegisterUserAsync(UserInputDTO RegisterInputModel);
        Task<Response<TokensOutputDTO>> LoginUserAsync(LoginInpulDTO LoginInputModel);
        Task<Response<UpdateUserInputDTO>> UpdateUserAsync(UpdateUserInputDTO updateUserInputModelDTO, UserClaimModel userClaimModel);
        Task<Response<bool>> ResetPasswordAsync(ResetPasswordInputDTO model);
        Task<Response<string>> ForgotPasswordAsync(ForgotPasswordModel model);
        Task<Response<bool>> CheckForgotPasswordTokenAsync(CheckForgotPasswordTokenModel model);
        Task<Response<JwtTokenOutputDTO>> GetTokenDetailsAsync(string refreshToken);
        Task<Response<TokensOutputDTO>> GetDirectTokensAsync(string userId);
    }
}
