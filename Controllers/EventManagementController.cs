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
    public class EventManagementController : AdvancedController
    {
        public override bool RequiredAdmin { get { return true; } }

        public ActionResult Index()
        {
            var events = Db.Events.Include(x => x.Category).Include(x => x.CreatorUser).Include(x => x.LastModifierByUser);
            return View(events.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = Db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(Db.EventCategories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ShortDescription,Description,EventDate,Location,LocationCoordinates,CategoryId,Slug,CreatorUserId,CreateDateTime,LastModifierByUserId,LastModifyDateTime,IsActive")] Event @event, string imagePath = null)
        {
            if (ModelState.IsValid)
            {
                AddCreationInfo(@event);
                Db.Events.Add(@event);
                Db.SaveChanges();
                if (!string.IsNullOrWhiteSpace(imagePath))
                    @event.ImageSave(imagePath, EntityImageFormat.Original, EntityImageFormat.Cover, EntityImageFormat.Icon);
                RoutingFactory.RegisterRoutes(RouteTable.Routes);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(Db.EventCategories, "Id", "Name", @event.CategoryId);
            return View(@event);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = Db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(Db.EventCategories, "Id", "Name", @event.CategoryId);
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ShortDescription,Description,EventDate,Location,LocationCoordinates,CategoryId,Slug,CreatorUserId,CreateDateTime,LastModifierByUserId,LastModifyDateTime,IsActive")] Event @event, string imagePath = null)
        {
            if (ModelState.IsValid)
            {
                AddModificationInfo(@event);
                Db.Events.AddOrUpdate(@event);
                Db.SaveChanges();
                if (!string.IsNullOrWhiteSpace(imagePath))
                    @event.ImageSave(imagePath, EntityImageFormat.Original, EntityImageFormat.Cover, EntityImageFormat.Icon);
                RoutingFactory.RegisterRoutes(RouteTable.Routes);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(Db.EventCategories, "Id", "Name", @event.CategoryId);
            return View(@event);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = Db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            Db.Events.Remove(@event);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
