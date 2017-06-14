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
    public class ReviewsController : Controller
    {
        private MyContext db = new MyContext();

        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.DiveSpot).Include(r => r.User).OrderBy(x => x.DiveSpotId);
            return View(reviews.ToList());
        }

        // GET: Reviews/Create
        [Authorize]
        public ActionResult Create()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place");
            return View();
        }

        // POST: Reviews/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,ReviewText,Star,DiveSpotId")] Review review)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }

            // set the user email to book
            review.Email = (string)Session["Email"];

            if (review.Star > 5 || review.Star < 1)
            {
                ViewBag.InvalidStars = true;
                ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", review.DiveSpotId);
                return View(review);
            }
            if (db.Reviews.Where(x => x.DiveSpotId == review.DiveSpotId && x.Email == review.Email).Count() >= 1)
            {
                ViewBag.SamePlaceReview = true;
                ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", review.DiveSpotId);
                return View(review);
            }
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DiveSpotId = new SelectList(db.DiveSpots, "Id", "Place", review.DiveSpotId);
            return View(review);
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
