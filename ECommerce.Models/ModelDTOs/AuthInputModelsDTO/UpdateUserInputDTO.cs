
using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModelsDTO.AuthInputModelsDTO
{
    public class UpdateUserInputDTO: IIdentityModel
    {
        public string? Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
