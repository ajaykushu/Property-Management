using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Utility
{
    public class ReferrerAttribute : ResultFilterAttribute
    {
        public ReferrerAttribute()
        {
           
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            

            var controller = context.Controller as Controller;
            if (controller != null)
            {
                controller.ViewBag.Referer = context.HttpContext.Request.Headers["Referer"].ToString();
            }
        }
    }
}
