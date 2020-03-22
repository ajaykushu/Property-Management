using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;

namespace Utilities
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class FeatureBasedAuthorization : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Feature { get; set; }
        public FeatureBasedAuthorization(string feature) : base()
        {
            Feature = feature;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
            var user = context.HttpContext.User;
            var featurecheck = new HashSet<string>();
            if (!user.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new ContentResult()
                {
                    Content = "Unauthorized to Access"
                };
                return;
            }
            
            else if (Feature != null && Feature != "" && Feature.Contains(","))
            {
                var temp = Feature.Split(",");
                foreach (var item in temp)
                    featurecheck.Add(item);
            }
            else
                featurecheck.Add(Feature);
            var featureCliam = user.FindFirst(x => x.Type == "Feature" && featurecheck.Contains(x.Value));
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
