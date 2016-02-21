using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GovEventer.Core;
using GovEventer.Models;

namespace GovEventer.Controllers
{
    public class ContentCategoryController : AdvancedController
    {
        public ActionResult Index()
        {
            return null;
        }

        public ActionResult Detail(string slug)
        {
            var contentCategory = Db.ContentCategories.FirstOrDefault(x => x.Slug.Equals(slug));
            return View(contentCategory);
        }
    }
}
