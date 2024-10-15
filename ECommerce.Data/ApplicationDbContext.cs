
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.DataModels.ProductModel;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<JwtToken> JwtTokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
