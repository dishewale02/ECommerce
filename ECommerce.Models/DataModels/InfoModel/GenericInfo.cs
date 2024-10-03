
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DataModels.InfoModel
{
    public class GenericInfo
    {
        [Key]
        public string? Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
