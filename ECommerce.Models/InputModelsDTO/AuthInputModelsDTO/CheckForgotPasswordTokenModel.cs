using ECommerce.Models.DataModels.InfoModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.InputModelsDTO.AuthInputModelsDTO
{
    public class CheckForgotPasswordTokenModel
    {
        [Required]
        public string Token { get; set; }  
    }
}
