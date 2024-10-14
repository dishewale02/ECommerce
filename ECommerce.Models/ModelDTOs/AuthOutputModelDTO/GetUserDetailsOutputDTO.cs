using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModelsDTO.AuthOutputModelDTO
{
    public class GetUserDetailsOutputDTO : IIdentityModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Phone {  get; set; }
    }
}
