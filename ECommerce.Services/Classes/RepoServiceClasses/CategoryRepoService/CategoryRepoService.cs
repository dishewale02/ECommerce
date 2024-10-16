
using AutoMapper;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Repo.Interfaces.GenericRepoInterface;
using ECommerce.Services.Classes.RepoServiceClasses.GenericRepoServiceClass;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface;

namespace ECommerce.Services.Classes.RepoServiceClasses.ProductRepoServiceClass
{
    public class CategoryRepoService : GenericRepoService<ProductInputDTO, Product>, ICategoryRepoService
    {
        public CategoryRepoService(IGenericRepo<Product> genericRepo, IMapper mapper) : base(genericRepo, mapper)
        {
       
        }
    }
}
