using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DataModels.AuthDataModels
{
    public class User: GenericInfo
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        [Required]
        public string? Role { get; set; } = "User";

    }
}
