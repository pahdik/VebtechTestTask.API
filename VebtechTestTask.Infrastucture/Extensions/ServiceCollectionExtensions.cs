using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VebtechTestTask.Domain.Repositories.Interfaces;
using VebtechTestTask.Infrastucture.Data;
using VebtechTestTask.Infrastucture.Repositories;

namespace VebtechTestTask.Infrastucture.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDatabaseConnection(this IServiceCollection service, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
        service.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                    connectionString, b => b.MigrationsAssembly("VebtechTestTask.Infrastucture")));
 
        return service;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection service)
    {
        service.AddTransient<IUserRespository,UserRepository>();
        return service;
    }
}
