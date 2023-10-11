using VebtechTestTask.Domain.Entities.Base;

namespace VebtechTestTask.Domain.Entities;

public class User: BaseEntity
{
    public string UserName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }   
    public byte[] Password { get; set; }
    public byte[] Salt { get; set; }

    public List<Role> Roles { get; set; }
}
