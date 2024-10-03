
using ECommerce.Data;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.AuthRepoInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ECommerce.Repo.Classes.AuthRepoClasses
{
    public class AuthRepo : IAuthRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<User>> FindByEmailAsync(string email)
        {
            //check if input email id is null.
            if(email == null)
            {
                return new Response<User>("email id is blank.");
            }

            //find if database is having email id or not.
            User? foundUserInDatabase = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            //check if found user is null.
            if(foundUserInDatabase is null)
            {
                return new Response<User>("User is not already available.");
            }

            return new Response<User>(foundUserInDatabase);
        }

        public async Task<Response<User>> FindByUserNameAsync(string userName)
        {
            //check if input userName is null.
            if (userName == null)
            {
                return new Response<User>("UserName is blank.");
            }

            //find if database is having email id or not.
            User? foundUserInDatabase = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            //check if found user is null.
            if (foundUserInDatabase is null)
            {
                return new Response<User>("User is not already available.");
            }

            return new Response<User>(foundUserInDatabase);
        }

        public async Task<Response<User>> RegisterAsync(User userModel)
        {
            //check if user is null or not.
            if (userModel is null)
            {
                return new Response<User>("input user is blank");
            }

            try
            {
                //save the user in database.
                EntityEntry<User> addedUserInDatabaseResponse = await _dbContext.Users.AddAsync(userModel);
                await SaveAsync();

                //extract saved User form response.
                User addedUserInDatabase = addedUserInDatabaseResponse.Entity;

                //check if user saved in database or not.
                if (addedUserInDatabase is null)
                {
                    return new Response<User>("error saving user in database.");
                }

                return new Response<User>(addedUserInDatabase);
            }
            catch (Exception ex)
            {
                return new Response<User>(ex.Message);
            }
        }

        private async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
