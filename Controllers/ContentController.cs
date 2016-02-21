using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GovEventer.Core;
using GovEventer.Models;

namespace GovEventer.Controllers
{
    public class ContentController : AdvancedController
    {
        public ActionResult Detail(string slug)
        {
            Models.Content content = Db.Contents.FirstOrDefault(x => x.IsActive && x.Slug == slug);
            if (content == null) return HttpNotFound();
            return View(content);
        }
    }
}