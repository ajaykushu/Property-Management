using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using System;
using System.Net;

namespace Utilities
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class FeatureBasedAuthorization : AuthorizeAttribute, IAuthorizationFilter
    {
        public MenuEnum Feature { get; set; }

        public FeatureBasedAuthorization(MenuEnum feature) : base()
        {
            Feature = feature;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new ContentResult()
                {
                    Content = "Unauthorized to Access"
                };
                return;
            }
            else
            {
                var featureCliam = user.FindFirst(x => x.Type == "Feature" && x.Value.Equals(Feature.ToString()));
                if (featureCliam == null)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Result = new ContentResult()
                    {
                        Content = "Unauthorized to Access this Resource"
                    };
                    return;
                }
                else
                    return;
            }
        }
    }
}