using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mummies.Models.ViewModels;

namespace WayOfWaterBuffalo.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-info")]
    public class PaginationTagHelper : TagHelper
    {

        IUrlHelperFactory uhf;

        public PaginationTagHelper(IUrlHelperFactory temp)
        {
            uhf = temp;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext vc { get; set; }
        public PageInfo PageInfo { get; set; }
        public string PageAction { get; set; }
        public string PageClass { get; set; }
        public string PageSelected { get; set; }
        public string PageNormal { get; set; }
        public bool PageClassesEnabled { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper uh = uhf.GetUrlHelper(vc);

            TagBuilder final = new TagBuilder("div");

            for (int i = 1; i <= PageInfo.numPages; i++)
            {
                TagBuilder tb = new TagBuilder("a");
                tb.Attributes["href"] = uh.Action(PageAction, new { pageNum = i });
                if (PageClassesEnabled)
                {
                    tb.AddCssClass(PageClass);
                    if (i == PageInfo.currentPage)
                    {
                        tb.AddCssClass(PageSelected);
                    }
                    else
                    {
                        tb.AddCssClass(PageNormal);
                    }
                }
                tb.InnerHtml.Append((i).ToString());
                final.InnerHtml.AppendHtml(tb);
            }

            output.Content.AppendHtml(final.InnerHtml);
        }
    }
}
