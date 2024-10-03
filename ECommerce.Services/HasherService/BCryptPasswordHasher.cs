

using ECommerce.Models.ResponseModel;

namespace ShoppingCart.Services.HasherService
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public async Task<Response<string>> GenerateHashAsync(string password)
        {
            if (password == null)
            {
                return new Response<string>("password field is empty.");
            }
            string passwordHashed = BCrypt.Net.BCrypt.HashPassword(password);

            if (passwordHashed == null)
            {
                return new Response<string>("internal error");
            }
            return new Response<string>().AddStringValue(passwordHashed);
        }

        public Task<bool> VerifyPasswordAsync(string password, string passwordHash)
        {
            if (password == null)
            {
                return Task.FromResult(false);
            }

            //password validation through BCrypt tool;
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);

            if (isPasswordValid)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
