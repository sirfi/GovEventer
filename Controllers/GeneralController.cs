using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServerCompact;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GovEventer.Core;

namespace GovEventer.Controllers
{
    public class GeneralController : AdvancedController
    {
        public ActionResult Index(
            string date = null, string slug = null)
        {
            DateTime selectedDate;
            if (string.IsNullOrEmpty(date) || !DateTime.TryParse(date, out selectedDate))
                selectedDate = DateTime.Today;
            ViewBag.SelectedDate = selectedDate;
            ViewBag.Category = string.IsNullOrEmpty(slug)
                ? null
                : Db.EventCategories.FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
            return View(Db.Events.Include(x => x.Category).Where(x => (slug == null || x.Category.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase)) && x.IsActive && SqlCeFunctions.DateDiff("DAY", x.EventDate, selectedDate) == 0).OrderBy(x => x.EventDate).ToList());
        }

    }
}
