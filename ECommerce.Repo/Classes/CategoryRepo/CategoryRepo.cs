
using ECommerce.Data;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Repo.Classes.GenericRepoClass;
using ECommerce.Repo.Interfaces.CategoryRepoInterface;

namespace ECommerce.Repo.Classes.CategoryRepo
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


    }
}
