using AutoMapper;
using System.Net;
using VebtechTestTask.Application.DTO.Users;
using VebtechTestTask.Application.Helpers;
using VebtechTestTask.Application.Services.Interfaces;
using VebtechTestTask.Domain.Entities;
using VebtechTestTask.Domain.Repositories.Interfaces;
using VebtechTestTask.Shared;
using VebtechTestTask.Shared.PagedList;
using ApplicationException = VebtechTestTask.Application.Exceptions.ApplicationException;
using VebtechTestTask.Application.Extensions;
using VebtechTestTask.Domain.Enums;
using System.Runtime.InteropServices;
using System.Linq;
using System.Data;

namespace VebtechTestTask.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRespository _userRespository;
    private readonly IMapper _mapper;

    public UserService(IMapper mapper, IUserRespository userRespository)
    {
        _mapper = mapper;
        _userRespository = userRespository;
    }

    public async Task<UserDto> AddRoleAsync(int userId, string roleName)
    {
        var user = await _userRespository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new ApplicationException(
                $"User with id <{userId}> is not found.",
                (int)HttpStatusCode.BadRequest);
        }
        var roles = Enum.GetNames(typeof(RoleType));
        if (!roles.Contains(roleName))
        {
            throw new ApplicationException(
                $"Role with name {roleName} doesn't exist.",
                (int)HttpStatusCode.BadRequest);
        }

        await _userRespository.AddRoleAsync(user, (RoleType)Enum.Parse(typeof(RoleType), roleName));
        var newUser = await _userRespository.GetByIdAsync(userId);
        var mappedUser = _mapper.Map<User, UserDto>(newUser);
        return mappedUser;
    }

    public async Task<UserDto> AuthenticateAsync(SignInModel model)
    {
        var user = await _userRespository.GetByNameAsync(model.UserName);
        if (user is null)
        {
            throw new ApplicationException(
                $"User with username <{model.UserName}> is not found",
                (int)HttpStatusCode.Unauthorized);
        }

        if (!PasswordHelper.ValidatePassword(model.Password, user.Password, user.Salt))
        {
            throw new ApplicationException(
                $"Incorrect password.",
                (int)HttpStatusCode.Unauthorized);
        }

        var mappedUser = _mapper.Map<User, UserDto>(user);
        return mappedUser;
    }

    public async Task<UserDto> CreateUserAsync(CreateUpdateUserDto model)
    {
        var user = _mapper.Map<CreateUpdateUserDto, User>(model);
        if (string.IsNullOrEmpty(model.Password))
        {
            (user.Password, user.Salt) = PasswordHelper.HashPassword("11111111");
        }
        else
        {
            (user.Password, user.Salt) = PasswordHelper.HashPassword(model.Password);
        }

        var created = await _userRespository.CreateAsync(user);
        var fullUserInfo = await _userRespository.GetByIdAsync(created.Id);
        var  mappedUser = _mapper.Map<User,UserDto>(fullUserInfo);
        return mappedUser;
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRespository.GetByIdAsync(id);
        if(user is null)
        {
            throw new ApplicationException(
                $"User with id <{id}> is not found.",
                (int)HttpStatusCode.BadRequest);
        }
        await _userRespository.DeleteAsync(user);
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _userRespository.GetByIdAsync(id);
        if (user is null)
        {
            throw new ApplicationException(
                $"User with id <{id}> is not found.",
                (int)HttpStatusCode.NoContent);
        }

        var mappedUser = _mapper.Map<User, UserDto>(user);
        return mappedUser;
    }

    public async Task<PagedList<UserDto>> GetPagedListAsync(PagingOptions pagingOptions, SortingOptions sortingOptions,
        FilteringOptions filteringOptions)
    {
        IsValid(sortingOptions,filteringOptions);

        var roleFilter = new List<Role>();
        if (filteringOptions.RoleFilter != "")
        {
            foreach (var role in filteringOptions.RoleFilter.Split(','))
            {
                roleFilter.Add(new Role { Type = (RoleType)Enum.Parse(typeof(RoleType), role.Trim()) });
            }
        }

        var users = await _userRespository.GetPagedListAsync(
            predicate: user =>
                (user.UserName.Contains(filteringOptions.NameFilter) && 
                user.Email.Contains(filteringOptions.EmailFilter) &&
                !user.Roles.Any(ur => roleFilter.Select(rf => (int)rf.Type).Contains((int)ur.Type)) && 
                user.Age > filteringOptions.MinAge && 
                user.Age < filteringOptions.MaxAge),
            orderBy: u => u.OrderBy(sortingOptions.Field, sortingOptions.Sort),
            pageSize: pagingOptions.PageSize,
            pageIndex: pagingOptions.PageIndex);
        return _mapper.Map<PagedList<User>, PagedList<UserDto>>(users);
    }

    public bool IsEmailUnique(string email)
    {
        return _userRespository.IsEmailUnique(email);
    }

    public async Task<UserDto> RegisterUserAsync(SignUpModel model)
    {
        var createUpdateUserDto = _mapper.Map<SignUpModel, CreateUpdateUserDto>(model);
        var newUser = await CreateUserAsync(createUpdateUserDto);
        var user = await AddRoleAsync(newUser.Id, "User");
        return user;
    }

    public async Task<UserDto> UpdateUserAsync(CreateUpdateUserDto model, int id)
    {
        var existingUser = await _userRespository.GetByIdAsync(id);
        if(existingUser is null)
        {
            throw new ApplicationException(
                $"User with id <{id}> is not found.",
                (int)HttpStatusCode.BadRequest);
        }

        _mapper.Map(model, existingUser);

        var updated = await _userRespository.UpdateAsync(existingUser);
        var fullUserInfo = await _userRespository.GetByIdAsync(updated.Id);
        var mappedUser = _mapper.Map<User,UserDto>(fullUserInfo);
        return mappedUser;
    }

    private void IsValid(SortingOptions sortingOptions, FilteringOptions filteringOptions)
    {
        var roleNames = Enum.GetNames(typeof(RoleType)).ToList();
        roleNames.Add("");
        foreach (var role in filteringOptions.RoleFilter.Split(","))
        {
            if (!roleNames.Contains(role.Trim()))
            {
                throw new ApplicationException(
                $"Role with name <{role.Trim()}> doesn't exist",
                (int)HttpStatusCode.BadRequest);
            }
        }

        var allFields = typeof(User).GetProperties().Select(f => f.Name).ToList();
        allFields.Remove("Roles");
        allFields.Add("");

        if (!allFields.Contains(sortingOptions.Field))
        {
            throw new ApplicationException(
                $"Field with name <{sortingOptions.Field}> doesn't exist",
                (int)HttpStatusCode.BadRequest);
        }
    }
}
