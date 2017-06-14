using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DiveCenterRhodes.Models;

namespace DiveCenterRhodes.Controllers
{
    public class InstructorsController : Controller
    {
        private MyContext db = new MyContext();

        // GET: Instructors
        //Only amdin can reach
        [Authorize]
        public ActionResult Index()
        {
            User u = db.Users.Find((string)Session["Email"]);

            if (u == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            if (u.Admin == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(db.Instructors.ToList());
        }

        // GET: Instructors/Details/5
        //Only amdin can reach
        [Authorize]
        public ActionResult Details(int? id)
        {
            User u = db.Users.Find((string)Session["Email"]);

            if (u == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            if (u.Admin == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructors/Create
        //Only amdin can reach
        [Authorize]
        public ActionResult Create()
        {
            User u = db.Users.Find((string)Session["Email"]);

            if (u == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            if (u.Admin == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Level")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instructor);
        }

        // GET: Instructors/Edit/5
        //Only amdin can reach
        [Authorize]
        public ActionResult Edit(int? id)
        {
            User u = db.Users.Find((string)Session["Email"]);

            if (u == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            if (u.Admin == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Level")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        //Only amdin can reach
        [Authorize]
        public ActionResult Delete(int? id)
        {
            User u = db.Users.Find((string)Session["Email"]);

            if (u == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            if (u.Admin == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
