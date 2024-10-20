
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Repo.Interfaces.ProductRepoInterface
{
    public interface IProductRepo
    {
        Task<Response<IEnumerable<Product>>> GetProductsByCategoryRepoAsync(string? category);
    }
}
