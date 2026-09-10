using System.Diagnostics;

namespace DiagnosisRepositoryApi.Configuration;

public class MiddlewarePipeline
{
    public class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var watch = Stopwatch.StartNew();

            await next(context);

            logger.LogInformation("{Metod} {Path} -> {Status} w {Elapsed} ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            watch.ElapsedMilliseconds);
        }
    }

    public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Nieobsługiwany wyjątek dla {Metod} {Path}",
                context.Request.Method, context.Request.Path);

                await Results.Problem(statusCode: 500, title: "Something went wrong")
                    .ExecuteAsync(context);
            }
        }
    }
}
