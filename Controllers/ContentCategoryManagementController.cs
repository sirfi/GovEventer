using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GovEventer.Core;
using GovEventer.Models;

namespace GovEventer.Controllers
{
    public class ContentCategoryManagementController : AdvancedController
    {
        public override bool RequiredAdmin { get { return true; } }

        public ActionResult Index()
        {
            var contentCategories = Db.ContentCategories.Include(c => c.CreatorUser).Include(c => c.LastModifierByUser);
            return View(contentCategories.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,CreatorUserId,CreateDateTime,LastModifierByUserId,LastModifyDateTime,IsActive")] ContentCategory contentCategory)
        {
            if (ModelState.IsValid)
            {
                AddCreationInfo(contentCategory);
                Db.ContentCategories.Add(contentCategory);
                Db.SaveChanges();
                RoutingFactory.RegisterRoutes(RouteTable.Routes); 
                return RedirectToAction("Index");
            }

            return View(contentCategory);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentCategory contentCategory = Db.ContentCategories.Find(id);
            if (contentCategory == null)
            {
                return HttpNotFound();
            }
            return View(contentCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,CreatorUserId,CreateDateTime,LastModifierByUserId,LastModifyDateTime,IsActive")] ContentCategory contentCategory)
        {
            if (ModelState.IsValid)
            {
                AddModificationInfo(contentCategory);
                Db.ContentCategories.AddOrUpdate(contentCategory);
                Db.SaveChanges();
                RoutingFactory.RegisterRoutes(RouteTable.Routes);
                return RedirectToAction("Index");
            }
            return View(contentCategory);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentCategory contentCategory = Db.ContentCategories.Find(id);
            if (contentCategory == null)
            {
                return HttpNotFound();
            }
            Db.ContentCategories.Remove(contentCategory);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
