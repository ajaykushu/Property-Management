using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly ICache _cache;
        public BlackListCheckMiddleware(RequestDelegate requestDelegate, ICache cache)
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
                var retval = _cache.GetItem(id);
                if (retval==null)
                {
                    //authenticated but blacklisted
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync("Request Failed");
                }
            }
            await _requestDelegate(httpContext);
        }
    }
}