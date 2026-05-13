using AutoMapper;
using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Core.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<RegisterUserDto, User>()
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
        .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.MobileNumber))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())  // set manually
        .ForMember(dest => dest.Role, opt => opt.Ignore())// set manually
        .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
    }
}
