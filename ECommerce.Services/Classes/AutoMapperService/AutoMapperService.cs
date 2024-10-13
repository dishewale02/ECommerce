using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;

namespace ECommerce.Services.Classes.AutoMapperService
{
    public class AutoMapperService : Profile
    {
        public AutoMapperService()
        {
            CreateMap<UserInputDTO, User>().ReverseMap();
            CreateMap<UserOutputDTO, User>().ReverseMap();
            CreateMap<JwtTokenDTO, JwtToken>().ReverseMap();
            CreateMap<UpdateUserInputModelDTO, UserOutputDTO>().ReverseMap();
            CreateMap<UpdateUserInputModelDTO, User>().ReverseMap();
            CreateMap<UpdateUserOutputModelDTO, User>().ReverseMap();
        }
    }
}
