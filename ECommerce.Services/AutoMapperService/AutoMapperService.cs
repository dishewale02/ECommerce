using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.DTOsModels.AuthDTOsModels;
using ECommerce.Models.InputModels.AuthInputModels;

namespace ShoppingCart.Services.MapperService
{
    public class AutoMapperService: Profile
    {
        public AutoMapperService()
        {
            CreateMap<RegisterInputModel, User>().ReverseMap();
        }
    }
}
