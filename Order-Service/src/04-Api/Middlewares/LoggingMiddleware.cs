using System.Diagnostics;

namespace Order_Service.src._04_Api.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            _logger.LogInformation("Request {Method} {Path} started.", context.Request.Method, context.Request.Path);

            try
            {
                await _next(context);
            }
            finally
            {
                watch.Stop();
                _logger.LogInformation("Request {Method} {Path} completed with status {StatusCode} in {ElapsedMilliseconds}ms.",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    watch.ElapsedMilliseconds);
            }
        }
    }
}
