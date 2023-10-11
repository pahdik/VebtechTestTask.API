using VebtechTestTask.Application.DTO.Users;
using VebtechTestTask.Shared;
using VebtechTestTask.Shared.PagedList;

namespace VebtechTestTask.Application.Services.Interfaces;

public interface IUserService
{
    Task<PagedList<UserDto>> GetPagedListAsync(PagingOptions pagingOptions, SortingOptions sortingOptions, FilteringOptions filteringOptions);
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> UpdateUserAsync(CreateUpdateUserDto model, int id);
    Task DeleteUserAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUpdateUserDto model);
    Task<UserDto> AddRoleAsync(int userId, string roleName);
    Task<UserDto> AuthenticateAsync(SignInModel model);
    Task<UserDto> RegisterUserAsync(SignUpModel model);
    bool IsEmailUnique(string email);
}
