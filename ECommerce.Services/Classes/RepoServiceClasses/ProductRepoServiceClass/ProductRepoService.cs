
using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ModelDTOs.ProductOutputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.GenericRepoInterface;
using ECommerce.Services.Classes.RepoServiceClasses.GenericRepoServiceClass;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface;

namespace ECommerce.Services.Classes.RepoServiceClasses.ProductRepoServiceClass
{
    public class ProductRepoService : GenericRepoService<ProductInputDTO, Product>, IProductRepoService
    {
        public ProductRepoService(IGenericRepo<Product> genericRepo, IMapper mapper) : base(genericRepo, mapper)
        {
        }

        public async Task<Response<ProductOutputDTO>> CreateProductAsync(ProductInputDTO productInputDTO, UserClaimModel userClaimModel)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<UpdateProductOutputDTO>> UpdateProductAsync(UpdateProductInputDTO updateProductInputDTO, UserClaimModel userClaimModel)
        {
            throw new NotImplementedException();
        }
    }
}
