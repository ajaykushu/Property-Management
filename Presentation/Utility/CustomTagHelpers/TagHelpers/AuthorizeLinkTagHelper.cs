using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Presentation.Utility.Interface;

namespace Presentation.Utility.CustomTagHelpers.TagHelpers
{
    [HtmlTargetElement("auth-link")]
    public class AuthorizeLinkTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionStorage _sessionStorage;
        public string feature { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
        public string bclass { get; set; }
        public string content { get; set; }
        public Dictionary<string,string> routedata { get; set; }
       
        public AuthorizeLinkTagHelper(IHttpContextAccessor httpContextAccessor, ISessionStorage sessionStorage)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionStorage = sessionStorage;
        }
       
        public override  void Process(TagHelperContext context, TagHelperOutput output)
        {
           
            output.TagName = "CustumTagHelper";
            output.TagMode = TagMode.StartTagAndEndTag;
            var route = String.Empty;
            if (routedata != null)
            {
                route = QueryString.Create(routedata).Value;
                
            }
            var sb = new StringBuilder();
            if(CheckAuthorizarion())
            sb.AppendFormat(@"<a class='{0}' href='/{1}/{2}{4}'>{3}</a>",bclass,controller,action,content,route);

            output.PreContent.SetHtmlContent(sb.ToString());
           
        }
        //check authorization
        public bool CheckAuthorizarion()
        {
            HashSet<string> menus=null;
            long Id = Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("UId"));
            menus = (HashSet<string>)_sessionStorage.GetItem(Id);
            if (menus!=null && menus.Contains(feature))
                return true;
            return false;
        }
    }
}
