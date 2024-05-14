using AutoMapper;
using QuickServe.Application.ViewModels.Roles;
using QuickServe.Application.ViewModels.UserDTO;
using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Infrastructure.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {

           
            CreateMap<CreatedUserDTO, User>();
            CreateMap<CreatedUserDTO, UserDTO>();
            CreateMap<UpdateProfileDTO, User>().ReverseMap();
            CreateMap<ProfileUserDTO, User>().ReverseMap();
            CreateMap<User, RegisterUserResponse>().ReverseMap();

            // Mapping giữa User và UserDTO, bao gồm Role
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ReverseMap();

            // Mapping giữa Role và RoleDTO
            CreateMap<Role, RoleDTO>().ReverseMap();

            CreateMap<RegisterUserDTO, User>();
            CreateMap<RegisterUserDTO, UserDTO>();

           
        }
    }
}
