
using ECommerce.Models.DataModels.AuthDataModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
        public virtual DbSet<JwtToken> JwtTokens { get; set; }
    }
}
