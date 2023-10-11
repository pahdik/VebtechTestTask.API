using Microsoft.AspNetCore.Mvc;
using VebtechTestTask.API.Controllers.Base;
using VebtechTestTask.Application.DTO.Users;
using VebtechTestTask.Application.Services.Implementations;
using VebtechTestTask.Application.Services.Interfaces;

namespace VebtechTestTask.API.Controllers;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
public class AuthenticationController : ApiController
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public AuthenticationController(ITokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    /// <summary>
    /// Аутентификация пользователя и выдача JWT-токена.
    /// </summary>
    /// <param name="model">Модель для входа (SignInModel).</param>
    /// <returns>Пользователь и JWT-токен.</returns>
    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SignInAsync([FromBody] SignInModel model)
    {
        var user = await _userService.AuthenticateAsync(model);
        var token = _tokenService.GenerateJwtToken(user!);
        return Ok(new
        {
            user,
            token
        });
    }

    /// <summary>
    /// Регистрация нового пользователя и выдача JWT-токена.
    /// </summary>
    /// <param name="model">Модель для регистрации (SignUpModel).</param>
    /// <returns>Новый пользователь и JWT-токен.</returns>
    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SignUpAsync([FromBody] SignUpModel model)
    {
        var user = await _userService.RegisterUserAsync(model);
        var token = _tokenService.GenerateJwtToken(user);
        return Ok(new
        {
            user,
            token
        });
    }

}
