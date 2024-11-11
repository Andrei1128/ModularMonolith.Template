using Core.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Core.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }
}
