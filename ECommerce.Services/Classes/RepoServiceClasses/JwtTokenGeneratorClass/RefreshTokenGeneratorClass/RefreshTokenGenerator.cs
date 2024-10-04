
using ECommerce.Models.DataModels;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass.RefreshTokenGeneratorClass
{
    public class RefreshTokenGenerator
    {
        private readonly TokenWriter _tokenWriter;
        private readonly AuthenticationConfig _authenticationConfig;

        public RefreshTokenGenerator(TokenWriter tokenWriter, AuthenticationConfig authenticationConfig)
        {
            _tokenWriter = tokenWriter;
            _authenticationConfig = authenticationConfig;
        }

        public async Task<Response<string>> GetRefreshTokenAsync(User user)
        {
            Response<string> token = await _tokenWriter.GenerateTokenAsync(
            _authenticationConfig.RefreshTokenSectet,
            _authenticationConfig.Issuer,
            _authenticationConfig.Audience,
            _authenticationConfig.RefreshTokenExpirationMinutes);
            if (token == null)
            {
                return Response<string>.Failure("token generation error.");
            }
            return Response<string>.Success(token.Value);
        }

    }
    
    
}
