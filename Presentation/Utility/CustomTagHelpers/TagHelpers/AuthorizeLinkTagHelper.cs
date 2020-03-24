﻿using System;
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
        public string Feature { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Bclass { get; set; }
        public string Content { get; set; }
        public Dictionary<string,string> Routedata { get; set; }
       
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
            if (Routedata != null)
            {
                route = QueryString.Create(Routedata).Value;
                
            }
            var sb = new StringBuilder();
            if (CheckAuthorizarion()) {
                if(!string.IsNullOrWhiteSpace(Action) || !string.IsNullOrWhiteSpace(Controller))
                    sb.AppendFormat(@"<a class='{0}' href='/{1}/{2}{4}'>{3}</a>", Bclass, Controller, Action, Content, route);
                else
                    sb.AppendFormat(@"<a class='{0}'>{1}</a>", Bclass, Content);
            }

            output.PreContent.SetHtmlContent(sb.ToString());
           
        }
        //check authorization
        public bool CheckAuthorizarion()
        {
            HashSet<string> menus;
            long Id = Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("UId"));
            menus = (HashSet<string>)_sessionStorage.GetItem(Id);
            if (menus!=null && menus.Contains(Feature))
                return true;
            return false;
        }
    }
}
