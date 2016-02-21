using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GovEventer.Models;

namespace GovEventer.Core
{
    public static class RoutingFactory
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();

            var contentCategorySlugs = DatabaseFactory.Instance.ContentCategories.Select(x => x.Slug).ToList().Where(x=>!string.IsNullOrEmpty(x));
            foreach (var slug in contentCategorySlugs)
            {
                routes.MapRoute("ContentCategory_" + slug, slug, new { controller = "ContentCategory", action = "Detail", slug }
               );
            }

            var contentSlugs = DatabaseFactory.Instance.Contents.Select(x => x.Slug).ToList().Where(x => !string.IsNullOrEmpty(x));
            foreach (var slug in contentSlugs)
            {
                routes.MapRoute("Content_" + slug, slug, new { controller = "Content", action = "Detail", slug }
               );
            }

            var eventCategorySlugs = DatabaseFactory.Instance.EventCategories.Select(x => x.Slug).ToList().Where(x => !string.IsNullOrEmpty(x));
            foreach (var slug in eventCategorySlugs)
            {
                routes.MapRoute("EventCategory_" + slug, slug, new { controller = "General", action = "Index", slug }
               );
            }

            var eventSlugs = DatabaseFactory.Instance.Events.Select(x => x.Slug).ToList().Where(x => !string.IsNullOrEmpty(x));
            foreach (var slug in eventSlugs)
            {
                routes.MapRoute("Event_" + slug, slug, new { controller = "Event", action = "Detail", slug }
               );
            }

            RouteConfig.RegisterRoutes(routes);

        }
    }
}