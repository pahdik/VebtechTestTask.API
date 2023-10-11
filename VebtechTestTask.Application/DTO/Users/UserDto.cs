using VebtechTestTask.Application.DTO.Roles;

namespace VebtechTestTask.Application.DTO.Users;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public List<RoleDto> Roles { get; set; }
}
