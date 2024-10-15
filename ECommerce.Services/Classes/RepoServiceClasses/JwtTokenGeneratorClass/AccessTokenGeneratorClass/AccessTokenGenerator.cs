
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
            //check if user is null.
            if(user is null || user.Id is null || user.Email is null || user.UserName is null || user.Role is null)
            {
                return Response<string>.Failure("can not create token.");
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
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
