using Microsoft.EntityFrameworkCore;
using VebtechTestTask.Domain.Entities;
using VebtechTestTask.Domain.Enums;

namespace VebtechTestTask.Infrastucture.Data;

public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if(!await context.Roles.AnyAsync())
        {
           await context.Roles.AddRangeAsync(GetRoles());
            await context.SaveChangesAsync();
        }
    }

    static IEnumerable<Role> GetRoles()
    {
        return new List<Role> 
        {
        new Role(){ Type = RoleType.User},
        new Role(){ Type = RoleType.Admin},
        new Role(){ Type = RoleType.SuperAdmin},
        new Role(){ Type = RoleType.Support}
        };
    }
}
