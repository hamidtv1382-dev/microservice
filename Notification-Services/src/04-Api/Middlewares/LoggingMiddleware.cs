using System.Diagnostics;

namespace Notification_Services.src._04_Api.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next) // پارامتر متفاوت باشد
        {
            _next = next; // حالا فیلد کلاس ست می‌شود
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}");

            await _next(context); // حالا دیگر null نیست

            watch.Stop();

            Console.WriteLine($"[Response] {context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode} in {watch.ElapsedMilliseconds}ms");
        }
    }

}
