using FluentValidation;
using VebtechTestTask.Application.DTO.Users;
using VebtechTestTask.Application.Services.Implementations;
using VebtechTestTask.Application.Services.Interfaces;

namespace VebtechTestTask.API.Validators;

public class CreateUpdateUserDtoValidator : AbstractValidator<CreateUpdateUserDto>
{
    private readonly IUserService _userService;
    public CreateUpdateUserDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(u => u.UserName).NotEmpty();
        RuleFor(u => u.Password).NotEmpty();
        RuleFor(u => u.Email).NotEmpty();
        RuleFor(u => u.Age).Must(a => a >= 0).WithMessage("Age cannot be negative.");
        RuleFor(u => u.Email).Must(BeUniqueEmail).WithMessage("This email is already in use.");
    }

    private bool BeUniqueEmail(string email)
    {
        return _userService.IsEmailUnique(email);
    }
}
