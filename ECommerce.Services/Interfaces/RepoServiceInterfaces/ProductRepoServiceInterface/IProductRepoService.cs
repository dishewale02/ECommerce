
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface
{
    public interface IProductRepoService: IGenericRepoService<ProductDTO, Product>
    {
        public Task<Response<List<ProductDTO>>> GetAllSearchedProductsAsync(string searchString);
        public Task<Response<List<ProductDTO>>> GetProductsByCategoryId(string categoryId);
    }
}
