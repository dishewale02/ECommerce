

using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.ResponseModel;

namespace ShoppingCart.Services.HasherService
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public async Task<Response<string>> GenerateHashAsync(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return Response<string>.Failure("password field is empty.");
            }
            string passwordHashed = BCrypt.Net.BCrypt.HashPassword(password);

            if (passwordHashed == null)
            {
                return Response<string>.Failure("internal error");
            }
            return Response<string>.Success(passwordHashed);
        }

        public Response<bool> VerifyPasswordAsync(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return Response<bool>.Failure("password can not blank.");
            }

            //password validation through BCrypt tool;
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);

            if (isPasswordValid)
            {
                return Response<bool>.Success(true);
            }
            return Response<bool>.Failure("password is not valid.");
        }
    }
}
