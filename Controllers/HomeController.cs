using DiveCenterRhodes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DiveCenterRhodes.Controllers
{
    public class HomeController : Controller
    {
        private MyContext db = new MyContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult School()
        {
            ViewBag.Message = "The school page.";
            return View();
        }

        public ActionResult Dive()
        {
            ViewBag.Message = "The dive spots page.";
            return View();
        }

        // Sign Up and Login Actions

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(User user)
        {
            if(Session["Email"]!= null)
            {
                return RedirectToAction("Login");
            }
            try
            {
                // 5 digits only for postal code
                if (user.PostalCode <= 9999 || user.PostalCode > 99999)
                {
                    ViewBag.InvalidPostalCode = true;
                    return View(user);
                }

                if (db.Users.Find(user.Email) == null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.EmailExists = true;
                    return View(user);
                }
            }
            catch(System.Data.Entity.Infrastructure.DbUpdateException)
            {
                ViewBag.InvalidDate = true;
                return View(user);
            }
            
            
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(User user)
        {
            User u = db.Users.Find(user.Email);
            if (u == null)
            {

                return RedirectToAction("SignUp");
            }
            if (u.Password == user.Password)
            {
                if (u.Admin == false)
                {
                    // Simple user LogIn Successful
                    Session.Add("Email", user.Email);
                    Session.Add("Name", user.FirstName);
                    FormsAuthentication.SetAuthCookie((string)Session["Email"], false);
                    return RedirectToAction("UserAccount");
                }
                else
                {
                    // Admin LogIn Successful
                    Session.Add("Email", user.Email);
                    Session.Add("Admin", user.Admin);
                    FormsAuthentication.SetAuthCookie((string)Session["Email"], false);
                    return RedirectToAction("Index", "Bookings");
                }
                
            }
            else
            {
                ViewBag.NotMatching = true;
                return View(user);
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult UserAccount()
        {
            User u = db.Users.Find((string)Session["Email"]);
            if (u == null)
            {
                return RedirectToAction("LogIn");
            }

           
            var userBookings = db.Bookings.Include("DiveSpot").Include("Service").Where(x => x.Email == u.Email).ToList();
            UserAccountModel Account = new UserAccountModel(u, userBookings);
            ViewBag.TotalPrice = Account.TotalPrice();
            ViewBag.Coupon = db.Coupons.Find(Account.user.CouponId);
            
            return View(Account);
        }

        [Authorize]
        public ActionResult SeeMyReviews()
        {
            User u = db.Users.Find((string)Session["Email"]);
            if (u == null)
            {
                return RedirectToAction("LogIn");
            }

            var reviews = db.Reviews.Include("DiveSpot").Where(x => x.Email == u.Email).ToList();
            if (reviews.Count() == 0)
            {
                ViewBag.EmptyReviews = true;
            }
            else
            {
                ViewBag.EmptyReviews = false;
            }
            return View(reviews);

        }

       
    }
}