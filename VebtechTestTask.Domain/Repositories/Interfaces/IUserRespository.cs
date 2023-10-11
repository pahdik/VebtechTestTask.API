using System.Linq.Expressions;
using VebtechTestTask.Domain.Entities;
using VebtechTestTask.Domain.Enums;
using VebtechTestTask.Domain.Repositories.Base;
using VebtechTestTask.Shared.PagedList;

namespace VebtechTestTask.Domain.Repositories.Interfaces;

public interface IUserRespository 
    :IRepository<User>
{
    Task<PagedList<User>> GetPagedListAsync(
        Expression<Func<User, bool>>? predicate = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
        int pageIndex = 0,
        int pageSize = 20);

    Task<User> AddRoleAsync(User entity, RoleType role);
    Task<User> GetByNameAsync(string userName);
    bool IsEmailUnique(string email);
}
