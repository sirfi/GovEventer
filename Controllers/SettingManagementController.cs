using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GovEventer.Core;
using GovEventer.Models;

namespace GovEventer.Controllers
{
    public class SettingManagementController : AdvancedController
    {
        public override bool RequiredAdmin { get { return true; } }

        public ActionResult Index()
        {
            return View(SettingFactory.Dictionary);
        }

        public ActionResult Save(SettingKey key, string value)
        {
            key.Set(value);
            return RedirectToAction("Index");
        }
    }
}