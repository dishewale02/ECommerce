
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface
{
    public interface IProductRepoService: IGenericRepoService<ProductDTO, Product>
    {
        Task<Response<List<ProductDTO>>> GetAllSearchedProductsAsync(string category, string searchString);
        Task<Response<List<ProductDTO>>> GetProductsByCategoryId(string categoryId);
        Task<Response<List<ProductDTO>>> GetDeletedAndNonActiveProducts();
        Task<Response<ProductDTO>> ActivateDeletedProduct(string productid);
    }
}
