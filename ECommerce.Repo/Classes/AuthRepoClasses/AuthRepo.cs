
using ECommerce.Data;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Classes.GenericRepoClass;
using ECommerce.Repo.Interfaces.AuthRepoInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ECommerce.Repo.Classes.AuthRepoClasses
{
    public class AuthRepo : GenericRepo<User>, IAuthRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<User>> FindByEmailAsync(string email)
        {
            try
            {
                //check if input email id is null.
                if (string.IsNullOrWhiteSpace(email))
                {
                    return Response<User>.Failure("email id is blank.");
                }

                //find if database is having email id or not.
                User? foundUserInDatabase = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsActive == true && x.IsDeleted == false);

                //check if found user is null.
                if (foundUserInDatabase is null)
                {
                    return Response<User>.Failure("User not Found");
                }

                return Response<User>.Success(foundUserInDatabase);
            }
            catch (Exception ex)
            {
                return Response<User>.Failure(ex.Message);
            }
        }

        public async Task<Response<User>> FindByUserNameAsync(string userName)
        {
            try
            {
                //check if input userName is null.
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return Response<User>.Failure("UserName is blank.");
                }

                //find if database is having email id or not.
                User? foundUserInDatabase = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.IsActive == true && x.IsDeleted == false);

                //check if found user is null.
                if (foundUserInDatabase is null)
                {
                    return Response<User>.Failure("User not Found");
                }

                return Response<User>.Success(foundUserInDatabase);
            }
            catch (Exception ex)
            {
                return Response<User>.Failure(ex.Message);
            }
        }

        public async Task<Response<User>> CreateUserAsync(User userModel)
        {
            //check if user is null or not.
            if (userModel is null)
            {
                return Response<User>.Failure("input user is blank");
            }

            try
            {
                //save the user in database.
                EntityEntry<User> addedUserInDatabaseResponse = await _dbContext.Users.AddAsync(userModel);

                //extract saved User form response.
                User addedUserInDatabase = addedUserInDatabaseResponse.Entity;

                //check if user saved in database or not.
                if (addedUserInDatabase is null)
                {
                    return Response<User>.Failure("error saving user in database.");
                }

                await SaveAsync();

                return Response<User>.Success(addedUserInDatabase);
            }
            catch (Exception ex)
            {
                return Response<User>.Failure(ex.Message);
            }
        }

        public async Task<Response<User>> UpdateUserDataAsync(User model)
        {
            try
            {
                if (model is null)
                {
                    return Response<User>.Failure("model can not be blank.");
                }

                //update the user in database.
                User? updatedUserInDatabaseResponse = await _dbContext.Users.FindAsync(model.Id);

                if (updatedUserInDatabaseResponse is null)
                {
                    return Response<User>.Failure("User not found.");
                }

                updatedUserInDatabaseResponse = model;
                //save the canges.
                await SaveAsync();

                return Response<User>.Success(updatedUserInDatabaseResponse);
            }
            catch (Exception ex)
            {
                return Response<User>.Failure(ex.Message);
            }
        }

        private async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Response<JwtToken>> SaveTokenInDatabaseAsync(JwtToken jwtToken)
        {
            try
            {
                //check if input is null.
                if (jwtToken == null)
                {
                    return Response<JwtToken>.Failure("Input Tokens are null.");
                }

                //save JwtToken model into Database.
                EntityEntry<JwtToken> entityEntry = await _dbContext.JwtTokens.AddAsync(jwtToken);

                JwtToken addTokenModelInDatabase = entityEntry.Entity;

                //check if user saved in database or not.
                if (addTokenModelInDatabase is null)
                {
                    return Response<JwtToken>.Failure("error saving user in database.");
                }

                await SaveAsync();

                return Response<JwtToken>.Success(addTokenModelInDatabase);

            }
            catch (Exception ex)
            {
                return Response<JwtToken>.Failure($"Failed to save {ex.Message}");
            }
        }

        public async Task<Response<User>> FindUserAsync(string username)
        {
            try
            {
                //check if input userName is null.
                if (string.IsNullOrWhiteSpace(username))
                {
                    return Response<User>.Failure("UserName is blank.");
                }

                //find if database is having email id or not.
                User? foundUserInDatabase = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);

                //check if found user is null.
                if (foundUserInDatabase is null)
                {
                    return Response<User>.Failure("User not Found");
                }

                return Response<User>.Success(foundUserInDatabase);
            }
            catch (Exception ex)
            {
                return Response<User>.Failure(ex.Message);
            }
        }

        public async Task<Response<JwtToken>> GetTokenDetailsAsync(string refreshToken)
        {
            try
            {
                //find token details in database.
                JwtToken foundTokenDetails = await _dbContext.JwtTokens.FirstAsync(x => x.RefreshToken == refreshToken);

                //check resposne.
                if (foundTokenDetails is null)
                {
                    return Response<JwtToken>.Failure("refresh token invalid.");
                }

                return Response<JwtToken>.Success(foundTokenDetails);
            }
            catch(Exception ex)
            {
                return Response<JwtToken>.Failure(ex.Message);
            }
        }
        public async Task<Response<User>> FindUserByPhoneAsync(string phoneNumber)
        {
            try
            {
                //check if input userName is null.
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    return Response<User>.Failure("UserName is blank.");
                }

                //find if database is having email id or not.
                User? foundUserInDatabase = await _dbContext.Users.FirstOrDefaultAsync(x => x.Phone == phoneNumber && x.IsActive == true && x.IsDeleted == false);

                //check if found user is null.
                if (foundUserInDatabase is null)
                {
                    return Response<User>.Failure("User not Found");
                }

                return Response<User>.Success(foundUserInDatabase);
            }
            catch (Exception ex)
            {
                return Response<User>.Failure(ex.Message);
            }
        }
    }
}
