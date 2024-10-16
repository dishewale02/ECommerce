
using ECommerce.Models.DataModels.InfoModel;
using ECommerce.Models.DataModels.ProductModel;

namespace ECommerce.Models.ModelDTOs.CategoryInputModelDTO
{
    public class CategoryInputDTO: IIdentityModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
