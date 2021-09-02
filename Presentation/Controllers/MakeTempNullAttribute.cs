using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Presentation.Controllers
{
    internal class MakeTempNullAttribute  : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            filterContext.RouteData.Values.Remove("Success");
            filterContext.RouteData.Values.Remove("Error");
            

        }
    }
}