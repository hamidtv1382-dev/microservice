using Media_Service.src._02_Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Media_Service.src._04_Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = exception.Message;

            switch (exception)
            {
                case MediaUploadFailedException:
                case MediaFolderCreationFailedException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case MediaOwnerNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                Status = (int)statusCode,
                Message = message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
