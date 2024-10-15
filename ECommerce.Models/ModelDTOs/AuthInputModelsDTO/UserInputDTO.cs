using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModelsDTO.AuthInputModelsDTO
{
    public class UserInputDTO: IIdentityModel
    {
        public string? Id { get; set; }
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
        public string? PasswordHash { get; set; }
        [Required]
        public string? Phone { get; set; }
        public string? Role { get; set; }
    }
}
