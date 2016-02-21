using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GovEventer.Core;
using GovEventer.Models;

namespace GovEventer.Controllers
{
    public class UserManagementController : AdvancedController
    {
        public override bool RequiredAdmin { get { return true; } }

        public ActionResult Index()
        {
            return View(Db.Users.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EMailAddress,Password,PasswordHash,FullName,IsAdmin,CreateDateTime,LastModifyDateTime")] User user)
        {
            if (ModelState.IsValid)
            {
                user.CreateDateTime = DateTime.Now;
                user.PasswordHash = user.Password.Hash();
                Db.Users.Add(user);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = Db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EMailAddress,Password,PasswordHash,FullName,IsAdmin,CreateDateTime,LastModifyDateTime")] User user)
        {
            if (ModelState.IsValid)
            {
                user.LastModifyDateTime = DateTime.Now;
                if (!string.IsNullOrEmpty(user.Password) ^ string.IsNullOrEmpty(user.PasswordHash))
                    user.PasswordHash = user.Password.Hash();
                Db.Users.AddOrUpdate(user);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = Db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            Db.Users.Remove(user);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
