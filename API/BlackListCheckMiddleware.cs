using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.Interface;

namespace API
{
    internal class BlackListCheckMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IDistributedCache _cache;

        public BlackListCheckMiddleware(RequestDelegate requestDelegate, IDistributedCache cache)
        {
            _requestDelegate = requestDelegate;
            _cache = cache;
        }

        public async Task Invoke(HttpContext httpContext)
          {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var id = httpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
                var jti = httpContext.User.FindFirst(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                var retval =await _cache.GetAsync(id);
                if (retval != null)
                {
                    await _requestDelegate(httpContext);
                }
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync("Request Failed");
                }
            }
            else
            {
                await _requestDelegate(httpContext);
            }
        }
    }
}