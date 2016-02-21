using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using GovEventer.Models;

namespace GovEventer.Core
{
    public class ImageViewModel
    {
        public string FieldName { get; set; }
        public string Path { get; set; }
    }

    public static class HtmlExtensions
    {
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (var template in (from object key in htmlHelper.ViewContext.HttpContext.Items.Keys where key.ToString().StartsWith("_script_") select htmlHelper.ViewContext.HttpContext.Items[key]).OfType<Func<object, HelperResult>>())
            {
                htmlHelper.ViewContext.Writer.Write(template(null));
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string fieldName)
        {
            var entity = htmlHelper.ViewData.Model as BaseEntity;
            var path = entity == null ? null : entity.GetImagePath();
            return htmlHelper.Partial("_Image", new ImageViewModel { FieldName = fieldName, Path = path });
        }

        public static MvcHtmlString PagerView<T>(this HtmlHelper htmlHelper, IEnumerable<T> enumerable)
        {
            var c = htmlHelper.ViewContext.Controller as Controller;
            if (c == null) return MvcHtmlString.Empty;
            var routeValueDictionary = new RouteValueDictionary();
            c.RouteData.Values.CopyItemsTo(routeValueDictionary);
            c.Request.QueryString.CopyTo(routeValueDictionary);
            return htmlHelper.PagerView(enumerable, pn => c.Url.RouteUrl(routeValueDictionary));
        }

        public static MvcHtmlString PagerView<T>(this HtmlHelper htmlHelper, IEnumerable<T> enumerable,
            string controller, string action)
        {
            var c = htmlHelper.ViewContext.Controller as Controller;
            if (c == null) return MvcHtmlString.Empty;
            return htmlHelper.PagerView(enumerable, pn => c.Url.Action(action, controller,
                new RouteValueDictionary(new Dictionary<string, object> { { GeneralExtensions.PageNumberKey, pn } })));
        }

        public static MvcHtmlString PagerView<T>(this HtmlHelper htmlHelper, IEnumerable<T> enumerable, Func<int, string> link)
        {
            var pagedCollection = enumerable as IPagedCollection<T>;
            return pagedCollection == null ?
                MvcHtmlString.Empty :
                htmlHelper.Partial("_Pager", new PageViewModel { PageCount = pagedCollection.PageCount, ItemCount = pagedCollection.ItemCount, PageItemCount = pagedCollection.PageItemCount, PageNumber = pagedCollection.PageNumber, Link = link });
        }
    }

    public class PageViewModel
    {
        public int PageItemCount { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public int ItemCount { get; set; }

        public Func<int, string> Link { get; set; }
    }
}