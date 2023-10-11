using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VebtechTestTask.Domain.Entities;
using VebtechTestTask.Domain.Enums;
using VebtechTestTask.Domain.Repositories.Interfaces;
using VebtechTestTask.Infrastucture.Data;
using VebtechTestTask.Infrastucture.Extensions;
using VebtechTestTask.Shared.PagedList;

namespace VebtechTestTask.Infrastucture.Repositories;

public class UserRepository : IUserRespository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddRoleAsync(User entity, RoleType role)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var findedRole = _context.Roles.FirstOrDefault(r => r.Type == role);
        entity.Roles.Add(findedRole);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<User> CreateAsync(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var user = await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return user.Entity;
    }

    public async Task DeleteAsync(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByIdAsync(int Id)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(x => x.Id == Id);
        return user;
    }

    public async Task<User> GetByNameAsync(string userName)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.UserName == userName);
        return user;
    }

    public Task<PagedList<User>> GetPagedListAsync(
        Expression<Func<User, bool>>? predicate = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
        int pageIndex = 0,
        int pageSize = 20)
    {
        IQueryable<User> query = _context.Users;

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return orderBy is not null
            ?orderBy(query).ToPagedListAsync(pageIndex,pageSize)
            :query.ToPagedListAsync(pageIndex, pageSize);
    }

    public bool IsEmailUnique(string email)
    {
        ArgumentNullException.ThrowIfNull(email);
        return !_context.Users.Any(u => u.Email == email);
    }

    public async Task<User> UpdateAsync(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var user = _context.Users.Update(entity);
        await _context.SaveChangesAsync();
        return user.Entity;
    }
}
