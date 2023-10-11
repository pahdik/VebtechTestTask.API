using VebtechTestTask.API.Middlewares;

namespace VebtechTestTask.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseExeptionHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
