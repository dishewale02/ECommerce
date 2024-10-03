
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Repo.Interfaces.AuthRepoInterface
{
    public interface IAuthRepo
    {
        Task<Response<User>> RegisterAsync(User userModel);
        Task<Response<User>> FindByEmailAsync(string email);
        Task<Response<User>> FindByUserNameAsync(string userName);
    }
}
