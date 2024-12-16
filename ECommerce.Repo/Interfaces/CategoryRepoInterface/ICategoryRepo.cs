
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.GenericRepoInterface;

namespace ECommerce.Repo.Interfaces.CategoryRepoInterface
{
    public interface ICategoryRepo: IGenericRepo<Category>
    {
        Task<Response<List<Category>>> RGetAllCategoriesAsync();
        Task<Response<Category>> RGetCategoryByCategoryIdAsync(string categoryName);
    }
}
