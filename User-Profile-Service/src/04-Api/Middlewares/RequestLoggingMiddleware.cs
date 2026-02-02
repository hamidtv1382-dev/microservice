using System.Diagnostics;

namespace User_Profile_Service.src._04_Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}");

            await _next(context);

            watch.Stop();

            Console.WriteLine($"[Response] {context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode} in {watch.ElapsedMilliseconds}ms");
        }
    }
}
