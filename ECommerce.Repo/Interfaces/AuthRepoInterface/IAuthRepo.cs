
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Repo.Interfaces.AuthRepoInterface
{
    public interface IAuthRepo
    {
        Task<Response<User>> CreateUserAsync(User userModel);
        Task<Response<User>> FindByEmailAsync(string email);
        Task<Response<User>> FindByUserNameAsync(string userName);
        Task<Response<User>> UpdateUserDataAsync(User model);
        Task<Response<JwtToken>> SaveTokenInDatabaseAsync(JwtToken jwtToken);
        Task<Response<User>> FindUserAsync(string username);
        Task<Response<JwtToken>> GetTokenDetailsAsync(string refreshToken);
    }
}
