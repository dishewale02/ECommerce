
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.CategoryInputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface
{
    public interface IProductRepoService
    {
        Task<Response<IEnumerable<ProductInputDTO>>> GetProductsByCategoryServiceAsync(string categoryName);
        Task<Response<ProductInputDTO>> CreateAsync(ProductInputDTO productInputDTO, UserClaimModel userClaimModel);
    }
}
