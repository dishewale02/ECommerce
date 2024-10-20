
using ECommerce.Data;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Classes.GenericRepoClass;
using ECommerce.Repo.Interfaces.ProductRepoInterface;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ECommerce.Repo.Classes.ProductRepoClass
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        private readonly ApplicationDbContext _context;
        public ProductRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Response<IEnumerable<Product>>> GetProductsByCategoryRepoAsync(string? category)
        {
            //check the input string category.
            if(category == null)
            {
                return Response<IEnumerable<Product>>.Failure("input category can not null.");
            }

            //find weather the input category is available in database or not.
            Category? foundCategoryInfo = _context.Categories.FirstOrDefault(x => x.Name == category);

            //check response.
            if(foundCategoryInfo == null)
            {
                return Response<IEnumerable<Product>>.Failure("category not available in database.");
            }

            //find all the entries of product which is having this category name.
            IEnumerable<Product> foundProducts = _context.Products.Where(x => x.CategoryId == foundCategoryInfo.Id).ToList();

            //check if products found list is null.
            if(foundProducts is null)
            {
                return Response<IEnumerable<Product>>.Failure("no products are available in this category.");
            }

            return Response<IEnumerable<Product>>.Success(foundProducts);
        }

        
    }
}
