using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModelsDTO.AuthOutputModelDTO
{
    public class JwtTokenDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
