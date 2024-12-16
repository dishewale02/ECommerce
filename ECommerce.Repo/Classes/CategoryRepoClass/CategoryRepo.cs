
using ECommerce.Data;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Classes.GenericRepoClass;
using ECommerce.Repo.Interfaces.CategoryRepoInterface;

namespace ECommerce.Repo.Classes.CategoryRepoClass
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Response<List<Category>>> RGetAllCategoriesAsync()
        {
            //get category list from database.
            List<Category> resultCategories = _context.Categories.ToList();

            List<Category> responseCategories = new List<Category>();

            foreach (Category category in resultCategories)
            {
                List<Product> allProductsContainsCategory = _context.Products.Where(x => x.CategoryId == category.Id && !x.IsDeleted && x.IsActive).ToList();

                category.Products = allProductsContainsCategory;
                responseCategories.Add(category);
            }

            return Response<List<Category>>.Success(responseCategories);
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
