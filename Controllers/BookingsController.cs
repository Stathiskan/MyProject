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
    public class BookingsController : Controller
    {
        private MyContext db = new MyContext();

        // GET: Bookings
        // Only amdin can reach
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
            var bookings = db.Bookings.Include(b => b.DiveSpot).Include(b => b.Instructor).Include(b => b.Service).Include(b => b.User).OrderBy(x => x.Date).ThenBy(c => c.DiveSpotId).ThenBy(j => j.ServiceId);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/
        // Only amdin can reach
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
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.Place = db.DiveSpots.Find(booking.DiveSpotId);
            ViewBag.Instructor = db.Instructors.Find(booking.InstructorId);
            ViewBag.TypeOfDive = db.Services.Find(booking.ServiceId);
            ViewBag.FirstName = db.Users.Find(booking.Email);
            return View(booking);
        }

        // GET: Bookings/Create
        [Authorize]
        public ActionResult Create()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place");
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "TypeOfDive");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,ServiceId,DiveSpotId,Date,NumOfDives,Weight,Height,ShoeNumber,InstructorId")] Booking booking)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }

            try
            {
                // set the user email to book
                booking.Email = (string)Session["Email"];


                // check for date
                if (booking.Date < DateTime.Now || booking.Date < new DateTime(2017, 05, 01) || booking.Date > new DateTime(2017, 10, 30))
                {
                    ViewBag.InvalidDate = true;
                    ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", booking.DiveSpotId);
                    ViewBag.ServiceId = new SelectList(db.Services, "Id", "TypeOfDive", booking.ServiceId);
                    return View(booking);
                }

                // check for num of dives
                if (booking.NumOfDives <= 0)
                {
                    ViewBag.NumOfDives = true;
                    ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", booking.DiveSpotId);
                    ViewBag.ServiceId = new SelectList(db.Services, "Id", "TypeOfDive", booking.ServiceId);
                    return View(booking);
                }
                // check for availability
                if (db.Bookings.Where(x => x.DiveSpotId == booking.DiveSpotId && x.Date == booking.Date).Count() >= 10)
                {
                    ViewBag.NotAvailable = true;
                    ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", booking.DiveSpotId);
                    ViewBag.ServiceId = new SelectList(db.Services, "Id", "TypeOfDive", booking.ServiceId);
                    return View(booking);
                }

                //check if user books accidentally twice the same book 
                if (db.Bookings.Where(x => x.Email == booking.Email && x.Date == booking.Date && x.DiveSpotId == booking.DiveSpotId).Count() >= 1)
                {
                    ViewBag.SameBook = true;
                    ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", booking.DiveSpotId);
                    ViewBag.ServiceId = new SelectList(db.Services, "Id", "TypeOfDive", booking.ServiceId);
                    return View(booking);
                }


                User u = db.Users.Find((string)Session["Email"]);



                // check for prices
                if (booking.ServiceId == 2)// school. fix price = 300
                {
                    Service Service = db.Services.Find(2);
                    booking.TotalPrice = Service.Price;
                }
                else // dive. depends on coupon 
                {
                    Service Service = db.Services.Find(1);
                    switch (u.CouponId)
                    {
                        case 1:
                            booking.TotalPrice = booking.NumOfDives * (Service.Price - Service.Price * 0.05M);
                            u.CouponId = null;
                            db.SaveChanges();
                            break;
                        case 2:
                            booking.TotalPrice = booking.NumOfDives * (Service.Price - Service.Price * 0.2M);
                            u.CouponId = null;
                            db.SaveChanges();
                            break;
                        case 3:
                            booking.TotalPrice = booking.NumOfDives * (Service.Price - Service.Price * 0.5M);
                            u.CouponId = null;
                            db.SaveChanges();
                            break;
                        default:
                            booking.TotalPrice = booking.NumOfDives * Service.Price;
                            break;
                    }
                }

                // check for model state and update db
                if (ModelState.IsValid)
                {

                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", booking.DiveSpotId);
                ViewBag.ServiceId = new SelectList(db.Services, "Id", "TypeOfDive", booking.ServiceId);
                return View(booking);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                ViewBag.InvalidFormatDate = true;
                ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", booking.DiveSpotId);
                ViewBag.ServiceId = new SelectList(db.Services, "Id", "TypeOfDive", booking.ServiceId);
                return View(booking);
            }

        }

        // GET: Bookings/Edit/5
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
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", booking.DiveSpotId);
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "Name", booking.InstructorId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "TypeOfDive", booking.ServiceId);
            ViewBag.Email = new SelectList(db.Users, "Email", "FirstName", booking.Email);

            return View(booking);
        }

        // POST: Bookings/Edit
       
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,ServiceId,DiveSpotId,Date,NumOfDives,Weight,Height,ShoeNumber,TotalPrice,Paid,InstructorId")] Booking booking)
        {
            var u = db.Users.Find(booking.Email);

            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                //check for coupons
                var NumOfDives = db.Bookings.Where(x => x.Email == u.Email && x.ServiceId == 1 && x.Paid == true).Count(); // find times of dive service
                if (NumOfDives == 3)
                {
                    u.CouponId = 1;
                    db.SaveChanges();
                }
                if (NumOfDives == 8)
                {
                    u.CouponId = 2;
                    db.SaveChanges();
                }
                if (NumOfDives == 11)
                {
                    u.CouponId = 3;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", booking.DiveSpotId);
            ViewBag.InstructorId = new SelectList(db.Instructors, "Id", "Name", booking.InstructorId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "TypeOfDive", booking.ServiceId);
            ViewBag.Email = new SelectList(db.Users, "Email", "FirstName", booking.Email);
            return View(booking);
        }

        // GET: Bookings/Delete/5
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
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            ViewBag.Place = db.DiveSpots.Find(booking.DiveSpotId);
            ViewBag.Instructor = db.Instructors.Find(booking.InstructorId);
            ViewBag.TypeOfDive = db.Services.Find(booking.ServiceId);
            ViewBag.FirstName = db.Users.Find(booking.Email);
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
