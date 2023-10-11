using System.Text.Json;
using ApplicationException = VebtechTestTask.Application.Exceptions.ApplicationException;

namespace VebtechTestTask.API.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ApplicationException e)
        {
            await HandleException(context, e.Message, e.StatusCode);
        }
        catch (Exception e)
        {
            await HandleException(context, e.Message, 500);
        }
    }

    private async Task HandleException(HttpContext context, string error, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error
        }));
    }
}
