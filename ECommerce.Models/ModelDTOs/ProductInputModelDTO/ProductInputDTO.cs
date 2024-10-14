
using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ModelDTOs.ProductInputModelDTO
{
    public class ProductInputDTO: IIdentityModel
    {
        public string? Id { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public decimal? ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
    }
}
