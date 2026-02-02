using System.Diagnostics;

namespace Seller_Finance_Service.src._04_Api.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            // Log Request
            Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}");

            await _next(context);

            watch.Stop();

            // Log Response
            Console.WriteLine($"[Response] {context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode} in {watch.ElapsedMilliseconds}ms");
        }
    }
}
