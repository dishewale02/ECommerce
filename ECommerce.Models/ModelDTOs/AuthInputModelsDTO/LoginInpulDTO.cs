using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModelsDTO.AuthInputModelsDTO
{
    public class LoginInpulDTO
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
