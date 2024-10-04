
namespace ECommerce.Models.DataModels
{
    public class AuthenticationConfig
    {
        public string AccessTokenSectet { get; set; }
        public double AccessTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RefreshTokenSectet { get; set; }
        public double RefreshTokenExpirationMinutes { get; set; }
    }
}
