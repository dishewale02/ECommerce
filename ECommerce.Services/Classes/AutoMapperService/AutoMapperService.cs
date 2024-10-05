using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;

namespace ECommerce.Services.Classes.AutoMapperService
{
    public class AutoMapperService : Profile
    {
        public AutoMapperService()
        {
            CreateMap<RegisterInputDTO, User>().ReverseMap();

        }
    }
}
