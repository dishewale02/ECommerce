using ECommerce.Data;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Classes.GenericRepoClass;
using ECommerce.Repo.Interfaces.AdminRepoInterface;
using ECommerce.Repo.Interfaces.GenericRepoInterface;

namespace ECommerce.Repo.Classes.NewFolder
{
    public class AdminRepo: GenericRepo<User>, IAdminRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<User>> RActivateUserAsync(string userId)
        {
            try
            {
                //check input Id.
                if (string.IsNullOrEmpty(userId))
                {
                    return Response<User>.Failure("input entity can not be null.");
                }

                //check weather the id is available in database.
                Response<User> foundUserByIdResponse = await RGetAsync(userId);

                //check response.
                if (!foundUserByIdResponse.IsSuccessfull)
                {
                    return Response<User>.Failure("user not availble in database.");
                }

                //check if the user is already deleted.
                if(!foundUserByIdResponse.Value.IsDeleted)
                {
                    return Response<User>.Failure("User is not already delted.");
                }

                //activate user.
                foundUserByIdResponse.Value.IsActive = true;
                foundUserByIdResponse.Value.IsDeleted = false;

                //send this user to save in database.
                Response<User> updatedUserResponse = await RUpdateAsync(foundUserByIdResponse.Value);

                if (!updatedUserResponse.IsSuccessfull)
                {
                    return Response<User>.Failure("user activation failed.");
                }

                return Response<User>.Success(updatedUserResponse.Value);
            }
            catch (Exception ex)
            {
                return Response<User>.Failure(ex.Message);
            }
        }

        public async Task<Response<List<User>>> RGetDeletedAndNonActiveUsers()
        {
            try
            {
                //get all the active and non deleted users.
                List<User> foundDeletedAndNonActiveUsers = _dbContext.Users.Where(x => !x.IsActive && x.IsDeleted).ToList();

                //check response.
                if (foundDeletedAndNonActiveUsers.Count == 0)
                {
                    return Response<List<User>>.Failure("No are in deleted list.");
                }

                return Response<List<User>>.Success(foundDeletedAndNonActiveUsers);
            }
            catch (Exception ex) 
            {
                return Response<List<User>>.Failure(ex.Message);
            }
        }

        public async Task<Response<List<Product>>> RGetNonDeletedAndActiveProducts()
        {
            //get data from database.
            List<Product> users = _dbContext.Products.Where(x => !x.IsDeleted && x.IsActive).ToList();

            //send this list to service layer.
            return Response<List<Product>>.Success(users);
        }
    }
}
