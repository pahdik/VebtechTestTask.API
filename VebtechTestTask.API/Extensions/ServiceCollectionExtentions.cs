using FluentValidation;
using VebtechTestTask.API.Middlewares;
using VebtechTestTask.API.Validators;
using VebtechTestTask.Application.DTO.Users;

namespace VebtechTestTask.API.Extensions;

public static class ServiceCollectionExtentions
{
    public static void AddExceptionHandlingMiddleware(this IServiceCollection services)
    {
        services.AddScoped<ExceptionHandlingMiddleware>();
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateUpdateUserDto>, CreateUpdateUserDtoValidator>();

        return services;
    }
}
