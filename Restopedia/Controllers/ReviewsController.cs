using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Restopedia.Models;

namespace Restopedia.Controllers
{
    public class ReviewsController : Controller
    {
        private RestopediaEntities db = new RestopediaEntities();

        // GET: Reviews
        //public ActionResult Index()
        //{
        //    if (Session["UserId"]==null)
        //    {
        //        return RedirectToAction("Login", "Users");
        //    }
        //    else
        //    {
        //        int test = Convert.ToInt32(Session["UserId"]);
        //        var reviews = db.Reviews.Include(r => r.Restaurant).Include(r => r.User).Where(r => r.User.UserId == test);
        //        return View(reviews.ToList());
        //    }

        //}

        public ActionResult Index([Bind(Prefix = "id")]int restaurantId)
        {
            var restaurant = db.Restaurants.Find(restaurantId);
            if (restaurant != null)
            {
                return View(restaurant);
            }
            return HttpNotFound();
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "RestaurantId", "Name");
            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Username");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.UserId = Convert.ToInt32(Session["UserId"]);
            
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = review.RestaurantId });
            }
            //ViewBag.RestaurantId = new SelectList(db.Restaurants, "RestaurantId", "Name", review.RestaurantId);
            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", review.UserId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            //ViewBag.RestaurantId = new SelectList(db.Restaurants, "RestaurantId", "Name", review.RestaurantId);
            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", review.UserId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Review review)
        {
            if (ModelState.IsValid)
            {
                //review.UserId = Convert.ToInt32(Session["UserId"]);
                //review.RestaurantId = 
                //db.Entry(review).Property(x => x.RestaurantId).IsModified = false;
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List","Restaurants");
            }
            //ViewBag.RestaurantId = new SelectList(db.Restaurants, "RestaurantId", "Name", review.RestaurantId);
            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", review.UserId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review= db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("List","Restaurants");
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
