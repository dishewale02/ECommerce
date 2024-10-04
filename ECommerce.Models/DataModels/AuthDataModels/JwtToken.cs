
using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DataModels.AuthDataModels
{
    public class JwtToken: GenericInfo
    {
        [Key]
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
