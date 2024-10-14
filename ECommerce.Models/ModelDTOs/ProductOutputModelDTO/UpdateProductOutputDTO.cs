
using ECommerce.Models.DataModels.InfoModel;

namespace ECommerce.Models.ModelDTOs.ProductOutputModelDTO
{
    public class UpdateProductOutputDTO: IIdentityModel
    {
        public string? Id { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
    }
}
