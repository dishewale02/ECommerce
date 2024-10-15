
using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModelsDTO.AuthInputModelsDTO
{
    public class ResetPasswordInputDTO
    {

        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
