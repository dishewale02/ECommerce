
using ECommerce.Models.ResponseModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass
{
    public class TokenWriter
    {
        public async Task<Response<string>> GenerateTokenAsync(string securityKey, string issuer, string audience, double expirationMinutes, IEnumerable<Claim> claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            SecurityToken token = new JwtSecurityToken
                (
                    issuer,
                    audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(expirationMinutes),
                    signingCredentials
                );

            return Response<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
