
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ModelDTOs.CategoryModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.CategoryRepoServiceInterface
{
    public interface ICategoryRepoService: IGenericRepoService<CategoryDTO, Category>
    {
        Task<Response<CategoryDTO>> GetByCategoryNameAsync(string categoryName);
        Task<Response<List<CategoryDTO>>> GetAllCategoriesAsync();
    }
}
