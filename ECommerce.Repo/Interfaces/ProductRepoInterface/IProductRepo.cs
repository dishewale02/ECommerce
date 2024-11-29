using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.GenericRepoInterface;

namespace ECommerce.Repo.Interfaces.ProductRepoInterface
{
    public interface IProductRepo: IGenericRepo<Product>
    {
        Task<Response<List<Product>>> RSearchProductAsync(string searchString);
        Task<Response<List<Product>>> RSearchProductsByCategoryIdAsync(string categoryId);
    }
}
