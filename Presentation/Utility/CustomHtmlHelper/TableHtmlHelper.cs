using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Presentation.Utility.CustomHtmlHelper
{

    public static class TableHtmlHelper
    {
       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="htmlHelper"></param>
       /// <param name="data"></param>
       /// <param name="title"></param>
       /// <param name="titleclass"></param>
       /// <param name="tableclass"></param>
       /// <param name="bodyclass"></param>
       /// <param name="headclass"></param>
       /// <param name="isOperation"></param>
       /// <param name="operations">
       /// var operation={
       /// name,url,param[]
       /// }
       /// </param>
       /// <returns></returns>
        public static IHtmlContent BuildTable<T>(this IHtmlHelper htmlHelper,List<T> data,string title,string titleclass,string tableclass,string bodyclass,string headclass,bool isOperation,List<Operation> operations)
        {   //for style addition
            int i = 1;

            StringBuilder sb = new StringBuilder();
            sb.Append("<style>@media only screen and (max-width: 580px) {");
            

            var div = new TagBuilder("div");
            div.AddCssClass(titleclass);
            div.InnerHtml.Append(title);
            var table = new TagBuilder("table");
            var span = new TagBuilder("span");
            if (data != null && data.Count > 0)
            {
                var thead = new TagBuilder("thead");
                thead.AddCssClass(headclass);
                var tr = new TagBuilder("tr");
                //add header values
                data[0].GetType().GetProperties().ToList().ForEach(prop =>
                {
                    var th = new TagBuilder("th");
                    var attribute = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true)
      .Cast<DisplayNameAttribute>().SingleOrDefault();
                    var skipprop = prop.GetCustomAttributes(typeof(SkipPropertyAttribute), true).Cast<SkipPropertyAttribute>().SingleOrDefault();
                    if (skipprop == null)
                    {
                        string displayName = attribute != null ? attribute.DisplayName : prop.Name;
                        th.InnerHtml.Append(displayName);
                        tr.InnerHtml.AppendHtml(th);
                        //adding css for mobile view

                        sb.Append(".dynamicTable td:nth-of-type(" + i + "):before {content:\"" + displayName + "\"}"); 
                        i++;
                    }


                });
                sb.Append("}</style>");
                if (isOperation)
                {
                    var newth = new TagBuilder("th");
                    tr.InnerHtml.AppendHtml(newth);
                }
                thead.InnerHtml.AppendHtml(tr);

                var tbody = new TagBuilder("tbody");
                tbody.AddCssClass(bodyclass);

                //adding tbody vaues
                data.ForEach(obj =>
                {
                    var tr = new TagBuilder("tr");
                    string id = String.Empty;
                    obj.GetType().GetProperties().ToList().ForEach(prop =>
                    {
                        var td = new TagBuilder("td");
                        var skipprop = prop.GetCustomAttributes(typeof(SkipPropertyAttribute), true).Cast<SkipPropertyAttribute>().SingleOrDefault();
                        var key = prop.GetCustomAttributes(typeof(KeyAttribute), true).Cast<KeyAttribute>().SingleOrDefault();
                        if (key != null)
                        {
                            id = prop.GetValue(obj, null).ToString();
                        }
                        if (skipprop == null)
                        {
                            td.InnerHtml.Append(prop.GetValue(obj, null)?.ToString());
                            tr.InnerHtml.AppendHtml(td);
                        }

                    });
                    if (isOperation)
                    {
                        var newth = new TagBuilder("td");
                        var div = new TagBuilder("div");
                        div.Attributes.Add("style", "display:grid;grid-gap: 1px;grid-auto-flow: column;");
                        foreach (var item in operations)
                        {
                            var anch = new TagBuilder("a");
                            anch.Attributes.Add("data-toggle", "tooltip");
                            anch.Attributes.Add("title", item.Name);
                            anch.AddCssClass(item.buttonClass);
                            anch.Attributes.Add("href", item.Url + "?id=" + id);
                            var itag = new TagBuilder("i");
                            itag.AddCssClass(item.IconClass);
                            anch.InnerHtml.AppendHtml(itag);
                            //anch.InnerHtml.Append(item.Name);
                            div.InnerHtml.AppendHtml(anch);
                        }
                        newth.InnerHtml.AppendHtml(div);
                        tr.InnerHtml.AppendHtml(newth);
                    }
                    tbody.InnerHtml.AppendHtml(tr);
                });


                //table created
               
                table.AddCssClass(tableclass);
                table.InnerHtml.AppendHtml(thead);
                table.InnerHtml.AppendHtml(tbody);
            }
            else
            {
                span.InnerHtml.AppendHtml("<strong>No Record Found</strong>");
            }
            var result = "";
            

            using (var writer = new StringWriter())
            {
                
                    div.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    table.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    span.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    
                result = writer.ToString();
            }
            result += sb.ToString();
            return new HtmlString(result);
        }
         
    }
    public class Operation
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string IconClass { get; set; }
        public string buttonClass{get;set;}

    }
    }
