using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Presentation.ViewModels;
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
           // var data = input.Payload;
            StringBuilder sb = new StringBuilder();
            sb.Append("<style>@media only screen and (max-width: 580px) {");
            List<KeyValuePair<string, bool>> propertylist = new List<KeyValuePair<string, bool>>();

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
                    var readonlyattr = prop.GetCustomAttributes(typeof(ReadOnlyAttribute), true)
     .Cast<ReadOnlyAttribute>().SingleOrDefault();
                    if (skipprop == null)
                    {
                        string displayName = attribute != null ? attribute.DisplayName : prop.Name;
                        
                        th.InnerHtml.Append(displayName);
                        tr.InnerHtml.AppendHtml(th);
                        //adding css for mobile view
                        
                        propertylist.Add(new KeyValuePair<string, bool>(prop.Name, readonlyattr!=null? readonlyattr.IsReadOnly:false));
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
                            var span = new TagBuilder("span");
                            span.InnerHtml.Append(prop.GetValue(obj, null)?.ToString());
                            td.InnerHtml.AppendHtml(span);
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
                            if (item.IsRedirect == false)
                            {
                                var inputbu = new TagBuilder("button");
                                inputbu.AddCssClass(item.buttonClass+" editbut");
                                inputbu.Attributes.Add("data-toggle", "tooltip");
                                inputbu.Attributes.Add("title", item.Name);
                                inputbu.Attributes.Add("type", "button");
                                inputbu.InnerHtml.Append(item.Name);
                                var canbut = new TagBuilder("button");
                                canbut.AddCssClass("btn btn-danger btn-sm" + " cancel");
                                canbut.Attributes.Add("data-toggle", "tooltip");
                                canbut.Attributes.Add("hidden", "true");
                                canbut.Attributes.Add("title","Cancel");
                                canbut.Attributes.Add("type", "button");
                                canbut.InnerHtml.Append("Cancel");
                                div.InnerHtml.AppendHtml(canbut);
                                div.InnerHtml.AppendHtml(inputbu);
                            }
                            else
                            {
                                var anch = new TagBuilder("a");
                                anch.Attributes.Add("data-toggle", "tooltip");
                                anch.Attributes.Add("title", item.Name);
                                anch.AddCssClass(item.buttonClass);
                                anch.Attributes.Add("href", item.Url + "?id=" + id);
                                var itag = new TagBuilder("i");
                                itag.AddCssClass(item.IconClass);
                                anch.InnerHtml.AppendHtml(itag);
                                div.InnerHtml.AppendHtml(anch);
                            }
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

            //write the pagination html
            //var hrtag = new TagBuilder("hr");
            //hrtag.TagRenderMode = TagRenderMode.SelfClosing;
            //var navtag = new TagBuilder("nav");
            //var ultag = new TagBuilder("ul");
            //ultag.AddCssClass("pagination");
            ////adding prev button
            //var currenturl = htmlHelper.ViewContext.HttpContext.Request.Scheme;
            //var prevli = new TagBuilder("li");
            //prevli.AddCssClass("page-item");
            //if(input.CurrentPage==0)
            //    prevli.AddCssClass("disabled");
            //var prevanchor = new TagBuilder("a");
            //prevanchor.AddCssClass("page-link");
            //prevanchor.Attributes.Add("href", currenturl);
            //prevli.InnerHtml.AppendHtml(prevanchor);
            //ultag.InnerHtml.AppendHtml(prevli);
            //for (int k = 0; k < input.PageCount; k++)
            //{
            //    var litag = new TagBuilder("li");
            //    litag.AddCssClass("page-item");
            //    var anchor = new TagBuilder("a");
            //    anchor.AddCssClass("page-link");
            //    if (input.CurrentPage == i)
            //        litag.AddCssClass("active");
            //    else 
            //    anchor.Attributes.Add("href", currenturl);

            //    litag.InnerHtml.AppendHtml(anchor);
            //    ultag.InnerHtml.AppendHtml(litag);

            //}


            //adding row edit finctionality
            
            result += sb.ToString();
            if(data!=null && data.Any())
            result += GetRowEditScript(propertylist, data[0].GetType().Name);
            return new HtmlString(result);
        }

        public static string GetRowEditScript(List<KeyValuePair<string,bool>> data,string name)
        {
            var sb = new StringBuilder();
            var jsarray= "var jsarray=[";
            for(int i = 0; i < data.Count; i++)
            {
                var str = "{'name':'" + data[i].Key + "','readonly':'" + data[i].Value + "'},";
                jsarray += str;
                  
           }
           jsarray=jsarray[jsarray.Length - 1] == ',' ? jsarray.Substring(0, jsarray.Length - 1) : jsarray;
            jsarray += "];";
            sb.Append("<script>");
            sb.Append(@"" +
                "\n var name='" + name + "';var arr=[];var m=0;var count="+data.Count +";"+
                 jsarray +
                 "\n var monthmapp={" +
                 "'Jan':'01'," +
                 "'Feb':'02'," +
                 "'Mar':'03'," +
                 "'Apr':'04'," +
                 "'May':'05'," +
                 "'Jun':'06'," +
                 "'Jul':'07'," +
                 "'Aug':'08'," +
                 "'Sep':'09'," +
                 "'Oct':'10'," +
                 "'Nov':'11'," +
                 "'Dec':'12'" +
                 "};\n" +
                "$('.editbut').on('click',function(e){\n var k=0;" +
                "var cell=$(this).closest('tr').find('td');\n" +
                "for(var i=0;i<cell.length-1;i++ ){ \n" +
                "var values=cell[i].innerText;\n" +
                "for(item of cell[i].children){if(item.tagName=='SPAN')item.hidden=true}" +
                "var inp= document.createElement('input');\n" +
                "var patt = /^\\d{2}[/-]([a-zA-Z0-9]){2,3}[/-]\\d{2,4}/g;\n" +
                "if(patt.test(values)){\n" +
                  "inp.type='date';\n" +
                  "var dt=values.split('-');\n" +
                  "inp.value=dt[2]+'-'+monthmapp[dt[1]] +'-'+dt[0];\n" +
                "}\n" +
                 "else{ inp.type='text';inp.value=values;}\n" +

                 "if(jsarray[k]['readonly']=='True'){inp.readOnly=true;}\n" +

                "inp.name=name+\"[\"+parseInt(m)+\"].\"+jsarray[k]['name'];\n" +
                "inp.classList.add('form-control','form-control-sm');" +
                "cell[i].appendChild(inp);\n" +
                "k++;" +
                "} $(this).prop('hidden',true);\n var cancelbutton=$(this).closest('div').find('.cancel')[0];cancelbutton.hidden=false;m++;\n" +
                "});\n" +
                "\n" +
                "$('.cancel').click(function(){\n var t=-1;" +
                "var cell=$(this).closest('tr').find('td');\n" +
                "for(var i=0;i<cell.length-1;i++ ){\n" +
                "for(item of cell[i].children){if(item.tagName=='SPAN')item.hidden=false; \n if(item.tagName=='INPUT')item.remove();\n}" +
                "}" +
                "$(this).closest('div').find('.editbut')[0].hidden=false;$(this).closest('div').find('.cancel')[0].hidden=true;\n" +
                "var items= $('.dynamicTable').find('input');\n" +
                "items.each(function(key){\nif(key%(count)==0){t=t+1; } var re=/(\\d+)/gi; var res=null; res=re.exec(items[key].name);\n if(res){items[key].name=items[key].name.substring(0,res.index)+parseInt(t)+items[key].name.substring(res.index+1,items[key].name.length); res.lastIndex=0; }}); m=t+1;\n" +
                "});"
                ) ;
            sb.Append("</script>");
            return sb.ToString();
        }
         
    }

     
    public class Operation
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string IconClass { get; set; }
        public string buttonClass{get;set;}

        public bool IsRedirect { get; set; }

    }
    }
