
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DataModels.InfoModel
{
    public class GenericInfo: IIdentityModel
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
