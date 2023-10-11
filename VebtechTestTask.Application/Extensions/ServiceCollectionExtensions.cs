using Microsoft.Extensions.DependencyInjection;
using VebtechTestTask.Application.Services.Implementations;
using VebtechTestTask.Application.Services.Interfaces;

namespace VebtechTestTask.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddScoped<IUserService,UserService>();
        service.AddScoped<ITokenService,TokenService>();
        return service;
    }
}
