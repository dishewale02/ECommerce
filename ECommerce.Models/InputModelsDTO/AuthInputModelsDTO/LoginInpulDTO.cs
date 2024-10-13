using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.InputModelsDTO.AuthInputModelsDTO
{
    public class LoginInpulDTO
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
