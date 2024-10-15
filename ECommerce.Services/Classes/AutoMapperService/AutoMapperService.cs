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
            CreateMap<JwtTokenOutputDTO, JwtToken>().ReverseMap();
            CreateMap<UpdateUserInputDTO, UserOutputDTO>().ReverseMap();
            CreateMap<UpdateUserInputDTO, User>().ReverseMap();
            CreateMap<UpdateUserOutputDTO, User>().ReverseMap();
            CreateMap<UserInputDTO, UserOutputDTO>().ReverseMap();
            CreateMap<GetUserDetailsOutputDTO, User>().ReverseMap();
            CreateMap<UpdateUserInputDTO, UserInputDTO>().ReverseMap();
            CreateMap<JwtTokenOutputDTO, JwtToken>().ReverseMap();
            CreateMap<TokensOutputDTO, JwtToken>().ReverseMap();
        }
    }
}
