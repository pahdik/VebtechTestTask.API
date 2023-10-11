using System.Diagnostics.CodeAnalysis;
using VebtechTestTask.Domain.Entities;

namespace VebtechTestTask.Application.Helpers;

public class RoleEqualityComparer : IEqualityComparer<Role>
{
    public bool Equals(Role? r1, Role? r2)
    {
        if (r1.Type == r2.Type) return true;
        return false;
    }

    public int GetHashCode([DisallowNull] Role obj)
    {
        return obj.Type.GetHashCode();
    }
}
