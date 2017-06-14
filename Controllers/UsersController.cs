using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DiveCenterRhodes.Models;
using System.Data.Entity.Migrations;

namespace DiveCenterRhodes.Controllers
{
    public class UsersController : Controller
    {
        private MyContext db = new MyContext();

        // GET: Users
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

            var users = db.Users.Include(x => x.Coupon);
            
            return View(users.ToList());
        }

        // GET: Users/Details
        //Only amdin can reach
        [Authorize]
        public ActionResult Details(string email)
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
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(email);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Coupon = db.Coupons.Find(user.CouponId);
           
            return View(user);
        }



        // GET: Users/Edit/
        
        [Authorize]
        public ActionResult Edit()
        {
            
            User u = db.Users.Find((string)Session["Email"]);

            if (u == null)
            {
               
                return RedirectToAction("LogIn", "Home");
            }
            if (u.Email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.CouponId = new SelectList(db.Coupons, "Id", "Type", u.CouponId);
           
            return View(u);
        }

        // POST: Users/Edit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FirstName,LastName,DateOfBirth,Level,PhoneNumber,Password,Country,Address,PostalCode,MedicalHistory,Admin,CouponId")] User user)
        {
            User u = db.Users.Find((string)Session["Email"]);

            if (u == null)
            {
                return RedirectToAction("LogIn", "Home");
            }

            user.Email = u.Email;
            user.CouponId = u.CouponId;
            user.Admin = u.Admin;

            // 5 digits only for postal code
            if (user.PostalCode <= 9999 || user.PostalCode > 99999)
            {
                ViewBag.InvalidPostalCode = true;
                ViewBag.CouponId = new SelectList(db.Coupons, "Id", "Type", user.CouponId);
                return View(user);
            }

            if (ModelState.IsValid)
            {

                
                db.Set<User>().AddOrUpdate(user);
                
                db.SaveChanges();
                if(u.Admin)
                {
                    return RedirectToAction("Index", "Bookings");
                }
                else
                {
                    return RedirectToAction("UserAccount", "Home");
                }
                
            }
            ViewBag.CouponId = new SelectList(db.Coupons, "Id", "Type", user.CouponId);
            return View(user);
        }

        // GET: Users/Delete
        //Only amdin can reach
        [Authorize]
        public ActionResult Delete(string email)
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
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(email);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.Coupon = db.Coupons.Find(user.CouponId);
            return View(user);
        }

        // POST: Users/Delete
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string email)
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
            User user = db.Users.Find(email);
            db.Users.Remove(user);
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
