using ePizza.Application.DTOs.Common;
using ePizza.Application.Exceptions;
using System.Net;

namespace ePizza.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var traceId = context.Items["TraceId"];

                await _next(context);
            }
            catch (RecordNotFoundException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errorResponse = GetApiErrorModelDto(context, ex, (int)HttpStatusCode.BadRequest, "The record doesn't exists in database");
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorResponse = GetApiErrorModelDto(context, ex, (int)HttpStatusCode.InternalServerError, "Something went wrong");
                await context.Response.WriteAsJsonAsync(errorResponse);
            }

        }



        private ApiErrorModelDto GetApiErrorModelDto(HttpContext context,
            Exception ex,
            int statusCode,
            string title)
        {
            var problem = new ApiErrorModelDto()
            {
                StackTrace = _env.IsEnvironment("Production") ? ex.ToString() : "Something went wrong",
                Title = title,
                Detail = ex.Message,
                StatusCode = statusCode
            };

            return problem;
        }

    }
}
