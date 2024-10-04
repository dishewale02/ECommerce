

using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.ResponseModel;

namespace ShoppingCart.Services.HasherService
{
    public interface IPasswordHasher
    {
        Task<Response<string>> GenerateHashAsync(string password);
        Response<User> VerifyPasswordAsync(string password, string passwordHash);
    }
}
