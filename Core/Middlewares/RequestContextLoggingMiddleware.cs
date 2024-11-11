using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace Core.Middlewares;

public class RequestContextLoggingMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context)
    {
        context.Request.Headers.TryGetValue("Correlation-Id", out StringValues correlationId);

        string correlationIdValue = correlationId.FirstOrDefault() ?? context.TraceIdentifier;

        using (LogContext.PushProperty("CorrelationId", correlationIdValue))
        {
            return next.Invoke(context);
        }
    }
}
