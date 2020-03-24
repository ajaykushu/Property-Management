using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.Interface;

namespace API
{
    internal class BlackListCheckMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<BlackListCheckMiddleware> _logger;
        private readonly ICache _cache;
        public BlackListCheckMiddleware(RequestDelegate requestDelegate, ILogger<BlackListCheckMiddleware> logger, ICache cache)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
            _cache = cache;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var id = httpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
                var token = _cache.GetItem(id);
                if (!string.IsNullOrWhiteSpace(id) && token.Equals(await httpContext.GetTokenAsync("access_token")))
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