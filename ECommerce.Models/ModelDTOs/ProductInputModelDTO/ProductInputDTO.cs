
using ECommerce.Models.DataModels.InfoModel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ModelDTOs.ProductInputModelDTO
{
    public class ProductInputDTO: IIdentityModel
    {
        public string? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public List<IFormFile?>? Images { get; set; }
        public string? ImagePaths { get; set; }
        public string? CategoryId { get; set; }
    }
}
