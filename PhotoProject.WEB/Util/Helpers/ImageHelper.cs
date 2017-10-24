using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoProject.WEB.Util.Helpers
{
    public static class ImageHelper
    {
        public static IHtmlString Image(this HtmlHelper helper, byte[] image, string imgclass,
                                     object htmlAttributes = null)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("class", imgclass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var imageString = image != null ? Convert.ToBase64String(image) : "";
            var img = string.Format("data:image/jpg;base64,{0}", imageString);
            builder.MergeAttribute("src", img);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}