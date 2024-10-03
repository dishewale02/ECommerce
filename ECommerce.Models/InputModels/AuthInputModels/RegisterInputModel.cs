using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModels.AuthInputModels
{
    public class RegisterInputModel
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
        public string? Phone { get; set; }
    }
}
