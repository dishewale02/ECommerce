
using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ModelDTOs.ProductInputModelDTO
{
    public class ProductDTO: IIdentityModel
    {
        public string? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
    }
}
