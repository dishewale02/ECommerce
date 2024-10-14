using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass.AccessTokenGeneratorClass;
using ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass.RefreshTokenGeneratorClass;
using ECommerce.Services.Interfaces.OtherServicesInterfaces.JwtTokenGeneratorInterface;

namespace ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass
{
    public class Authenticator : IAuthenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;

        public Authenticator(AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<Response<TokensOutputDTO>> GenerateJwtTokensAsync(User user)
        {
            //check if input user is null.
            if(user is null)
            {
                return Response<TokensOutputDTO>.Failure("null user.");
            }

            //generate access token.
            Response<string> accessTokenGeneratorResponse = await _accessTokenGenerator.GetAccessTokenAsync(user);

            if(!accessTokenGeneratorResponse.IsSuccessfull)
            {
                return Response<TokensOutputDTO>.Failure(accessTokenGeneratorResponse.ErrorMessage);
            }

            //generate refresh token.
            Response<string> refreshTokenGeneratorResponse = await _refreshTokenGenerator.GetRefreshTokenAsync(user);

            if(!refreshTokenGeneratorResponse.IsSuccessfull)
            {
                return Response<TokensOutputDTO>.Failure(refreshTokenGeneratorResponse.ErrorMessage);
            }

            //create new JwtToken instance and save above tokens into it.
            TokensOutputDTO jwtTokenDTO = new TokensOutputDTO();

            jwtTokenDTO.AccessToken = accessTokenGeneratorResponse.Value;
            jwtTokenDTO.RefreshToken = refreshTokenGeneratorResponse.Value;

            return Response<TokensOutputDTO>.Success(jwtTokenDTO);
        }
    }
}
