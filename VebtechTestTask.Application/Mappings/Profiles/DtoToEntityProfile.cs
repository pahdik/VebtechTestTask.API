using AutoMapper;
using VebtechTestTask.Application.DTO.Users;
using VebtechTestTask.Domain.Entities;

namespace VebtechTestTask.Application.Mappings.Profiles;

public class DtoToEntityProfile : Profile
{
    public DtoToEntityProfile()
    {
        CreateMap<CreateUpdateUserDto, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}
