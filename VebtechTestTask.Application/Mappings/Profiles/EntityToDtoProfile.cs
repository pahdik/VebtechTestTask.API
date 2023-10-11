using AutoMapper;
using VebtechTestTask.Application.DTO.Roles;
using VebtechTestTask.Application.DTO.Users;
using VebtechTestTask.Domain.Entities;
using VebtechTestTask.Domain.Enums;
using VebtechTestTask.Shared.PagedList;

namespace VebtechTestTask.Application.Mappings.Profiles;

public class EntityToDtoProfile : Profile
{
    public EntityToDtoProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<PagedList<User>, PagedList<UserDto>>();
        CreateMap<Role, RoleDto>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.Type.ToString()));
        CreateMap<SignUpModel, CreateUpdateUserDto>();
    }
}
