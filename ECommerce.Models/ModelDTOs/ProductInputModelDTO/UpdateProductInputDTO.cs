
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ModelDTOs.ProductInputModelDTO
{
    public class UpdateProductInputDTO
    {
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public decimal? ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
    }
}
