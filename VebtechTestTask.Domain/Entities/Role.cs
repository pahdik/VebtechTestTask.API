using VebtechTestTask.Domain.Entities.Base;
using VebtechTestTask.Domain.Enums;

namespace VebtechTestTask.Domain.Entities;

public class Role: BaseEntity 
{ 
    public RoleType Type { get; set; }
    
    public List<User> Users { get; set; }

}