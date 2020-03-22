using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Presentation
{
    public class CustomAuthorize
    {
        private readonly RequestDelegate _requestDelegate;
        public CustomAuthorize(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;


        }

        public async Task Invoke(HttpContext httpContext)
        {

            if (httpContext.Session.TryGetValue("token", out var token))
            {
                httpContext.Request.Headers.Add("Authorization", "Bearer " + httpContext.Session.GetString("token"));
            }
            await _requestDelegate(httpContext);
        }
    }
}