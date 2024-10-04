
using ECommerce.Models.DataModels;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.ResponseModel;
using System.Security.Claims;

namespace ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass.AccessTokenGeneratorClass
{
    public class AccessTokenGenerator
    {
        private readonly TokenWriter _tokenWriter;
        private readonly AuthenticationConfig _authenticationConfig;


        public AccessTokenGenerator(TokenWriter tokenWriter, AuthenticationConfig authenticationConfig)
        {
            _tokenWriter = tokenWriter;
            _authenticationConfig = authenticationConfig;
        }

        public async Task<Response<string>> GetAccessTokenAsync(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            Response<string> token = await _tokenWriter.GenerateTokenAsync(
                _authenticationConfig.AccessTokenSectet,
                _authenticationConfig.Issuer,
                _authenticationConfig.Audience,
                _authenticationConfig.AccessTokenExpirationMinutes,
                claims);

            if(token is null)
            {
                return Response<string>.Failure("Token Generation Error.");
            }

            return Response<string>.Success(token.Value);
        }
    }
}
