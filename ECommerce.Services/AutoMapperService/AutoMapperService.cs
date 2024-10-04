using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;

namespace ShoppingCart.Services.MapperService
{
    public class AutoMapperService: Profile
    {
        public AutoMapperService()
        {
            CreateMap<RegisterInputDTO, User>().ReverseMap();

        }
    }
}
