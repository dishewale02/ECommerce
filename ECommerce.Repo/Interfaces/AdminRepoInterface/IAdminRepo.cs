using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Repo.Interfaces.AdminRepoInterface
{
    public interface IAdminRepo
    {
        Task<Response<User>> RActivateUserAsync(string userId);
        Task<Response<List<User>>> RGetDeletedAndNonActiveUsers();
        Task<Response<List<Product>>> RGetNonDeletedAndActiveProducts();
    }
}
