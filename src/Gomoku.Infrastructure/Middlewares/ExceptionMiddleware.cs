using Gomoku.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Gomoku.Infrastructure.Middlewares;
public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        // todo: validation exceptions
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            GameMoveExistsException => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status400BadRequest
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error = exception.Message
        }));
    }
}
