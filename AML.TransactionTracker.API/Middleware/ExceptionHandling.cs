using AML.TransactionTracker.Application.Exceptions;
using AML.TransactionTracker.Core.Exceptions;
using System.Net;

namespace AML.TransactionTracker.API.Middleware
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);

    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;

        public ExceptionHandling(RequestDelegate next)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ExceptionResponse response = exception switch
            {
                InvalidDateException => new ExceptionResponse(HttpStatusCode.BadRequest, "Cannot use future date."),
                InvalidEntityIdException => new ExceptionResponse(HttpStatusCode.BadRequest, "Incorrect id send."),
                EntityAlreadyExistsException => new ExceptionResponse(HttpStatusCode.BadRequest, "Entity already exists."),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
