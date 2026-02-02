using Messaging_Service.src._02_Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Messaging_Service.src._04_Api.Middlewares
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

            var response = new ProblemDetails
            {
                Title = "An error occurred",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = exception.Message
            };

            switch (exception)
            {
                case ChatNotFoundException:
                case MessageNotFoundException: // Assuming this exists in your domain/app exceptions
                    response.Status = (int)HttpStatusCode.NotFound;
                    response.Title = "Resource Not Found";
                    break;

                case MessageNotApprovedException:
                case MessageFailedException:
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Title = "Operation Failed";
                    break;

                default:
                    response.Status = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode = response.Status.Value;

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
