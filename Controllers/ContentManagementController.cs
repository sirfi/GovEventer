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
    public class ContentManagementController : AdvancedController
    {
        public override bool RequiredAdmin { get { return true; } }

        public ActionResult Index()
        {
            var contents = Db.Contents.Include(c => c.Category).Include(c => c.CreatorUser).Include(c => c.LastModifierByUser);
            return View(contents.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Content content = Db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(Db.ContentCategories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Body,CategoryId,Slug,CreatorUserId,CreateDateTime,LastModifierByUserId,LastModifyDateTime,IsActive")] Models.Content content)
        {
            if (ModelState.IsValid)
            {
                AddCreationInfo(content);
                Db.Contents.Add(content);
                Db.SaveChanges();
                RoutingFactory.RegisterRoutes(RouteTable.Routes);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(Db.ContentCategories, "Id", "Name", content.CategoryId);
            return View(content);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Content content = Db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(Db.ContentCategories, "Id", "Name", content.CategoryId);
            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Body,CategoryId,Slug,CreatorUserId,CreateDateTime,LastModifierByUserId,LastModifyDateTime,IsActive")] Models.Content content)
        {
            if (ModelState.IsValid)
            {
                AddModificationInfo(content);
                Db.Contents.AddOrUpdate(content);
                Db.SaveChanges();
                RoutingFactory.RegisterRoutes(RouteTable.Routes);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(Db.ContentCategories, "Id", "Name", content.CategoryId);
            return View(content);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Content content = Db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            Db.Contents.Remove(content);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
