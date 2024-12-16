
using ECommerce.Models.DataModels.InfoModel;
using ECommerce.Models.DataModels.ProductModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ModelDTOs.CategoryModelDTO
{
    public class CategoryDTO: IIdentityModel
    {
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ProductCount { get; set; }
    }
}
