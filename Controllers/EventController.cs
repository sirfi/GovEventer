using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GovEventer.Core;

namespace GovEventer.Controllers
{
    public class EventController : AdvancedController
    {
        public ActionResult Detail(string slug)
        {
            Models.Event content = Db.Events.Include(x=>x.Category).FirstOrDefault(x => x.IsActive && x.Slug == slug);
            if (content == null) return HttpNotFound();
            return View(content);
        }
    }
}