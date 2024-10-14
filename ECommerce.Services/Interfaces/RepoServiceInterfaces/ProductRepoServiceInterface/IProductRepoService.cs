
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ModelDTOs.ProductOutputModelDTO;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface
{
    public interface IProductRepoService
    {
        Task<Response<ProductOutputDTO>> CreateProductAsync(ProductInputDTO productInputDTO, UserClaimModel userClaimModel);
        Task<Response<UpdateProductOutputDTO>> UpdateProductAsync(UpdateProductInputDTO updateProductInputDTO, UserClaimModel userClaimModel);
    }
}
