using ECommerce.Models.DataModels.InfoModel;

namespace ECommerce.Models.InputModelsDTO.AuthOutputModelDTO
{
    public class UserClaimModel : IIdentityModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
