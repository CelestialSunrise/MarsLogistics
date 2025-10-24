using System.Net;
using System.Text.Json;

namespace MarsLogistics.Middleware
{
    public class MiddleHandleException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddleHandleException> _logger;
        public MiddleHandleException(RequestDelegate next, ILogger<MiddleHandleException> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //Log the actions                
                await _next(context);
            }
            catch (Exception ex) 
            {
                await HandleEception(context, ex);
            }
        }

        private async Task HandleEception(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode;
            string message;

            switch (ex)
            {
                //Log the exceptions
                case ArgumentNullException:
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = ex.Message;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = ex.Message;
                    break;
            }

            var response = new
            {
                status = (int)statusCode,
                response = message,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
