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
    public class EventCategoryManagementController : AdvancedController
    {
        public override bool RequiredAdmin { get { return true; } }

        public ActionResult Index()
        {
            var eventCategories = Db.EventCategories.Include(e => e.CreatorUser).Include(e => e.LastModifierByUser);
            return View(eventCategories.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Slug,CreatorUserId,CreateDateTime,LastModifierByUserId,LastModifyDateTime,IsActive")] EventCategory eventCategory)
        {
            if (ModelState.IsValid)
            {
                AddCreationInfo(eventCategory);
                Db.EventCategories.Add(eventCategory);
                Db.SaveChanges();
                RoutingFactory.RegisterRoutes(RouteTable.Routes); 
                return RedirectToAction("Index");
            }

            return View(eventCategory);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventCategory eventCategory = Db.EventCategories.Find(id);
            if (eventCategory == null)
            {
                return HttpNotFound();
            }
            return View(eventCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Slug,CreatorUserId,CreateDateTime,LastModifierByUserId,LastModifyDateTime,IsActive")] EventCategory eventCategory)
        {
            if (ModelState.IsValid)
            {
                AddModificationInfo(eventCategory);
                Db.EventCategories.AddOrUpdate(eventCategory);
                Db.SaveChanges();
                RoutingFactory.RegisterRoutes(RouteTable.Routes); 
                return RedirectToAction("Index");
            }
            return View(eventCategory);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventCategory eventCategory = Db.EventCategories.Find(id);
            if (eventCategory == null)
            {
                return HttpNotFound();
            }
            Db.EventCategories.Remove(eventCategory);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
