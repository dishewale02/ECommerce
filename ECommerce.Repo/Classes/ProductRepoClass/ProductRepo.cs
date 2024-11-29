using ECommerce.Data;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Classes.GenericRepoClass;
using ECommerce.Repo.Interfaces.ProductRepoInterface;

namespace ECommerce.Repo.Classes.ProductRepoClass
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public ProductRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Response<List<Product>>> RSearchProductAsync(string searchString)
        {
            try
            {
                //check input string.
                if (string.IsNullOrEmpty(searchString))
                {
                    return Response<List<Product>>.Failure("input search field is empty.");
                }

                IEnumerable<Product> searchedProductsBySearchString = _dbSet.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString));

                //check output.
                if(!searchedProductsBySearchString.Any())
                {
                    return Response<List<Product>>.Failure("No Product Found.");
                }

                return Response<List<Product>>.Success(searchedProductsBySearchString.ToList());
            }
            catch (Exception ex) 
            {
                return Response<List<Product>>.Failure(ex.Message);
            }
        }

        public async Task<Response<List<Product>>> RSearchProductsByCategoryIdAsync(string categoryId)
        {
            try
            {
                //check input string.
                if (string.IsNullOrEmpty(categoryId))
                {
                    return Response<List<Product>>.Failure("input search field is empty.");
                }

                

                IEnumerable<Product> searchedProductsByCategoryId = _dbSet.Where(x => x.CategoryId == categoryId);

                //check output.
                if (!searchedProductsByCategoryId.Any())
                {
                    return Response<List<Product>>.Failure("No Product Found.");
                }

                return Response<List<Product>>.Success(searchedProductsByCategoryId.ToList());
            }
            catch (Exception ex)
            {
                return Response<List<Product>>.Failure(ex.Message);
            }
        }
    }
}
