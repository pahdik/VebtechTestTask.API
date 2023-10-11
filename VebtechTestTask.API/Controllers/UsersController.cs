using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VebtechTestTask.API.Controllers.Base;
using VebtechTestTask.Application.DTO.Users;
using VebtechTestTask.Application.Services.Interfaces;
using VebtechTestTask.Domain.Entities;
using VebtechTestTask.Domain.Enums;
using VebtechTestTask.Shared;
using VebtechTestTask.Shared.PagedList;

namespace VebtechTestTask.API.Controllers;

/// <summary>
/// Контроллер для управления пользователями.
/// </summary>
[Authorize]
public class UsersController : ApiController
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUpdateUserDto> _validator;

    public UsersController(IUserService userService, IValidator<CreateUpdateUserDto> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    /// <summary>
    /// Получение пользователя по ID.
    /// </summary>
    /// <param name="id">ID пользователя.</param>
    /// <returns>Информация о пользователе.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    public async Task<ActionResult<UserDto>> GetByIdAsync(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    /// <summary>
    /// Создание нового пользователя.
    /// </summary>
    /// <param name="model">Модель создания пользователя.</param>
    /// <returns>Созданный пользователь.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    public async Task<ActionResult<UserDto>> CreateAsync([FromForm] CreateUpdateUserDto model)
    {
        var result = _validator.Validate(model);
        if (!result.IsValid) 
        {
            return UnprocessableEntity();
        }
        var user = await _userService.CreateUserAsync(model);
        return CreatedAtAction("GetById", new { id = user.Id }, user);
    }

    /// <summary>
    /// Обновление существующего пользователя.
    /// </summary>
    /// <param name="id">ID пользователя.</param>
    /// <param name="model">Модель обновления пользователя.</param>
    /// <returns>Обновленный пользователь.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    public async Task<ActionResult<UserDto>> UpdateAsync(int id, [FromForm] CreateUpdateUserDto model)
    {
        var result = _validator.Validate(model);
        if (!result.IsValid)
        {
            return UnprocessableEntity();
        }
        var user = await _userService.UpdateUserAsync(model, id);
        return Ok(user);
    }


    /// <summary>
    /// Удаление пользователя по ID.
    /// </summary>
    /// <param name="id">ID пользователя для удаления.</param>
    /// <returns>Результат операции удаления.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Получение списка пользователей с учетом параметров запроса.
    /// </summary>
    /// <param name="requestOptions">Параметры запроса для фильтрации, сортировки и пагинации.</param>
    /// <returns>Список пользователей, удовлетворяющих параметрам запроса.</returns>
    [HttpPost("GetPagedList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<PagedList<UserDto>>))]

    public async Task<ActionResult<IList<UserDto>>> GetAllAsync([FromForm] RequestOptions requestOptions)
    {
        var users = await _userService.GetPagedListAsync(new PagingOptions
            {
                PageIndex = requestOptions?.Page ?? 0,
                PageSize = requestOptions?.PageSize ?? 20
            },
            new SortingOptions
            {
                Sort = requestOptions?.Sort ?? "asc",
                Field = requestOptions?.SortField ?? "UserName"
            },
            new FilteringOptions
            {
                NameFilter = requestOptions?.NameFilter ?? "",
                EmailFilter = requestOptions?.EmailFilter ?? "",
                RoleFilter = requestOptions?.RoleFilter ?? "",
                MinAge = requestOptions?.MinAge ?? 0,
                MaxAge = requestOptions?.MaxAge ?? int.MaxValue
            });

        return Ok(users);
    }

    /// <summary>
    /// Добавление роли к пользователю.
    /// </summary>
    /// <param name="userId">ID пользователя, к которому добавляется роль.</param>
    /// <param name="roleName">Имя роли для добавления.</param>
    /// <returns>Пользователь с добавленной ролью.</returns>
    [HttpPost("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    public async Task<ActionResult<UserDto>> AddRoleToUser(int userId, [FromForm] string roleName)
    {
        var user = await _userService.AddRoleAsync(userId, roleName);
        return Ok(user);
    }
}
