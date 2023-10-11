using VebtechTestTask.Application.DTO.Users;

namespace VebtechTestTask.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(UserDto user);
}
