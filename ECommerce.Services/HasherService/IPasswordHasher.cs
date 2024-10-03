

using ECommerce.Models.ResponseModel;

namespace ShoppingCart.Services.HasherService
{
    public interface IPasswordHasher
    {
        Task<Response<string>> GenerateHashAsync(string password);
        Task<bool> VerifyPasswordAsync(string password, string passwordHash);
    }
}
