using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Presentation.Utility.CustomTagHelpers.TagHelpers
{
    [HtmlTargetElement("custimage")]
    public class ImageDisplayTagHelper : TagHelper
    {
        public List<KeyValuePair<string,string>> Images { get; set; }
        public KeyValuePair<string, string> Image { get; set; }
        public List<string> format { get; set; }
        public ImageDisplayTagHelper()
        {
            format = new List<string>();
            format.Add("jpeg");
            format.Add("png");
            format.Add("jpg");

        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            
            var sb = new StringBuilder();
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            if (Images != null && Images.Count>0)
            {
                foreach(var item in Images)
                {
                    if(format.Where(x=>item.Value.Contains(x, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault() != null)
                    {
                        sb.AppendFormat(@"<div class='photo_disp' style='display:block'>
                         <span class='OpenImageDisplayModal'>", item.Value);
                        if (String.IsNullOrEmpty(item.Value))
                        {
                            sb.AppendFormat(@"<img src='/NA.jpg' width='200' height='150'>");
                        }
                        else{
                            var thumb = item.Value.Replace(".jpg",".png");
                             sb.AppendFormat(@"<img src='{0}' width='200' height='150'>", thumb);
                        }
                        sb.AppendFormat(@"{0}</span>
                                        <input type = 'button' class='btn btn-sm btn-danger' onclick='AddItem(event)' name='{1}' value='Delete'>
                                    </div>",item.Key,item.Value);
                    }
                    else
                    {
                        sb.AppendFormat(@"<div class='photo_disp' style='display:block'>
                                  <a href ='{0}' >
                                  <img src='/File.jpg' width='200' height='150'>
                                           {1}
                                        </a>
                                        <input type = 'button' class='btn btn-sm btn-danger' onclick='AddItem(event)' name='{0}' value='Delete'>
                                    </div>", item.Value, item.Key);
                    }
                }
            }
            else
            {

            }
            output.Attributes.SetAttribute("style", "display:contents");
            output.Content.SetHtmlContent(sb.ToString());
        }
        }
}