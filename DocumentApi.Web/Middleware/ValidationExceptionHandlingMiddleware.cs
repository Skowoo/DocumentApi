using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Middleware
{
    public class ValidationExceptionHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                ProblemDetails details = new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "Validation Failure",
                    Title = "Validation Error",
                    Detail = "One or more validation errors has occurred"
                };

                if (ex.Errors is not null)
                    details.Extensions["Errors"] = ex.Errors;

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(details);
            }
        }
    }
}
