
using ECommerce.Data;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Classes.GenericRepoClass;
using ECommerce.Repo.Interfaces.CategoryRepoInterface;

namespace ECommerce.Repo.Classes.CategoryRepoClass
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        public CategoryRepo(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Response<Category>> RGetCategoryByCategoryIdAsync(string categoryName)
        {
            try
            {
                //check if input category name is correct.
                if (string.IsNullOrEmpty(categoryName))
                {
                    return Response<Category>.Failure("input field is blank.");
                }

                //find category information in datbase.
                Category? foundCategory = _dbSet.FirstOrDefault(x => x.Name == categoryName);

                //check category.
                if (foundCategory == null)
                {
                    return Response<Category>.Failure("category not available in database.");
                }

                return Response<Category>.Success(foundCategory);
            }
            catch (Exception ex)
            {
                return Response<Category>.Failure(ex.Message);
            }
        }
    }
}
