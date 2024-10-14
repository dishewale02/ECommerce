using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModelsDTO.AuthOutputModelDTO
{
    public class JwtTokenOutputDTO
    {
        public string? RefreshToken { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public DateTime RefreshTokenValidityTill { get; set; }
    }
}
