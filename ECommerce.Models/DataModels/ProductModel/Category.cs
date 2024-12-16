using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DataModels.ProductModel
{
    public class Category: GenericInfo
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Product>? Products { get; set; } = new List<Product>();
    }
}