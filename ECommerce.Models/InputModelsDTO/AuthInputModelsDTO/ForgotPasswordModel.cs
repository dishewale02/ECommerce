
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModelsDTO.AuthInputModelsDTO
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
