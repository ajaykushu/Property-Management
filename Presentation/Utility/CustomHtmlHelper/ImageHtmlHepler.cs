using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Utility.CustomHtmlHelper
{
    public static class ImageHtmlHepler
    {
        public static List<string> format { get; set; }
        static ImageHtmlHepler()
        {
            format = new List<string>();
            format.Add("jpeg");
            format.Add("png");
            format.Add("jpg");
        }

        public static IHtmlContent DisplayImage(this IHtmlHelper htmlHelper, List<KeyValuePair<string, string>> Images)
        {

            var div1 = new TagBuilder("div");
            div1.AddCssClass("customdisp");

            if (Images != null && Images.Count > 0)
            {
                foreach (var item in Images)
                {
                    if (format.Where(x => item.Value.Contains(x, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault() != null)
                    {
                        var div = new TagBuilder("div");
                        div.AddCssClass("photo_disp");
                        div.Attributes.Add("style", "display:inline-table;margin:5px;");
                        var thumb = item.Value.Replace(".jpg", ".png");
                        var span = new TagBuilder("span");
                        span.AddCssClass("OpenImageDisplayModal");
                        span.Attributes.Add("style", "word-break: break-word;");
                        var img = new TagBuilder("img");
                        img.TagRenderMode = TagRenderMode.SelfClosing;
                        img.Attributes.Add("width", "200");
                        img.Attributes.Add("class", "zoom-img");
                        img.Attributes.Add("height", "150");
                        img.Attributes.Add("src", thumb);
                        img.Attributes.Add("data-original", item.Value);
                        span.InnerHtml.AppendHtml(img);
                        span.InnerHtml.Append(item.Key);
                        var input = new TagBuilder("input");
                        input.TagRenderMode = TagRenderMode.SelfClosing;
                        input.Attributes.Add("type", "button");
                        input.AddCssClass("btn btn-sm btn-danger mt-1");
                        input.Attributes.Add("onclick", "AddItem(event)");
                        input.Attributes.Add("name", item.Value);
                        input.Attributes.Add("value", "Delete");
                        div.InnerHtml.AppendHtml(span);
                        div.InnerHtml.AppendHtml(input);
                        div1.InnerHtml.AppendHtml(div);
                    }
                    else
                    {
                        var div = new TagBuilder("div");
                        div.AddCssClass("photo_disp");
                        var anch = new TagBuilder("a");
                        anch.Attributes.Add("href", item.Value);
                        var img = new TagBuilder("img");
                        img.TagRenderMode = TagRenderMode.SelfClosing;
                        img.Attributes.Add("width", "200");
                        img.Attributes.Add("height", "150");
                        img.Attributes.Add("src", "/File.jpg");
                        anch.InnerHtml.Append(item.Key);
                        anch.InnerHtml.AppendHtml(img);
                        var input = new TagBuilder("input");
                        input.TagRenderMode = TagRenderMode.SelfClosing;
                        input.Attributes.Add("type", "button");
                        input.AddCssClass("btn btn-sm btn-danger");
                        input.Attributes.Add("onclick", "AddItem(event)");
                        input.Attributes.Add("name", item.Value);
                        input.Attributes.Add("value", "Delete");
                        div.InnerHtml.AppendHtml(anch);
                        div.InnerHtml.AppendHtml(input);
                        div1.InnerHtml.AppendHtml(div);

                    }
                }
            }
            string result;
            using (var writer = new StringWriter())
            {
                div1.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                result = writer.ToString();
            }
            return new HtmlString(result);
        }
    }
}
