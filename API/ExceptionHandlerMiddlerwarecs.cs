using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace API
{
    public class ExceptionHandlerMiddlerwarecs
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionHandlerMiddlerwarecs> _logger;

        public ExceptionHandlerMiddlerwarecs(RequestDelegate requestDelegate, ILogger<ExceptionHandlerMiddlerwarecs> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public RequestDelegate RequestDelegate => _requestDelegate;

        public Task Invoke(HttpContext httpContext)
        {
            
              var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                
                httpContext.Response.ContentType = "application/json";
                _logger.LogInformation(contextFeature.Error.Message + "\nStack Trace: " + contextFeature.Error.StackTrace);
                switch (contextFeature.Error.GetType().Name)
                {
                    case "BadRequestException":
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        httpContext.Response.WriteAsync(contextFeature.Error.Message);
                        break;

                    case "UnAuthorizedException":
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        httpContext.Response.WriteAsync(contextFeature.Error.Message);
                        break;

                    default:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        httpContext.Response.WriteAsync("Some Error Occured");
                        break;
                }
            }
            return Task.CompletedTask;
        }
    }
}