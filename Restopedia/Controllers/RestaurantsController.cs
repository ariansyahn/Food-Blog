using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Restopedia.Models;

namespace Restopedia.Controllers
{
    public class RestaurantsController : Controller
    {
        private RestopediaEntities db = new RestopediaEntities();

        // GET: Restaurants
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["RoleId"]) == 2 || Session["RoleId"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return View(db.Restaurants.OrderByDescending(Model => Model.RestaurantId).ToList());
            }     
        }

        public ActionResult List()
        {
            var model =
               db.Restaurants
                   .OrderByDescending(r => r.Reviews.Average(review => review.Rating))
                   //.Where(r => searchTerm == null || r.Name.Contains(searchTerm))
                   //.Take(10)
                   .Select(r => new RestaurantViewModel
                   {
                       RestaurantId = r.RestaurantId,
                       Name = r.Name,
                       City = r.City,
                       Image = r.Image,
                       Country = r.Country,
                       Address = r.Address,
                       CountOfReviews = r.Reviews.Count(),
                       AverageRating = r.Reviews.Average(review => review.Rating)
                   });
            return View(model);
            //return View(db.Restaurants.OrderByDescending(Model => Model.RestaurantId).ToList());
        }

        public ActionResult Single(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model =
               db.Restaurants
                   .OrderByDescending(r => r.Reviews.Average(review => review.Rating))
                   .Where(r => r.RestaurantId == id)
                   //.Where(r => searchTerm == null || r.Name.Contains(searchTerm))
                   //.Take(10)
                   .Select(r => new RestaurantViewModel
                   {
                       RestaurantId = r.RestaurantId,
                       Name = r.Name,
                       City = r.City,
                       Image = r.Image,
                       Country = r.Country,
                       Address = r.Address,
                       CountOfReviews = r.Reviews.Count(),
                       AverageRating = r.Reviews.Average(review => review.Rating)
                   });
            return View(model);
        }

        // GET: Restaurants/Details/5
        public ActionResult Details(int? id)
        {
            if (Convert.ToInt32(Session["RoleId"]) == 2 || Session["RoleId"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Restaurant restaurant = db.Restaurants.Find(id);
                if (restaurant == null)
                {
                    return HttpNotFound();
                }
                return View(restaurant);
            }
        }

        // GET: Restaurants/Create
        public ActionResult Create()
        {
            if (Convert.ToInt32(Session["RoleId"]) == 2 || Session["RoleId"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return View();
            }
        
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RestaurantId,Name,City,Country,Address,Image,Description")] Restaurant restaurant, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                string file_name = Path.GetFileName(image.FileName);

                string path = Path.Combine(Server.MapPath("~/Content/images/restaurants"), file_name);
                image.SaveAs(path);

                // image_path is nvarchar type db column. We save the name of the file in that column. 
                restaurant.Image = file_name;
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Convert.ToInt32(Session["RoleId"]) == 2 || Session["RoleId"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Restaurant restaurant = db.Restaurants.Find(id);
                if (restaurant == null)
                {
                    return HttpNotFound();
                }
                return View(restaurant);
            }
          
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Restaurant restaurant, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                //var model = db.Restaurants.Find(restaurant.RestaurantId);
                var model = restaurant;
                string oldfilePath = model.Image;

                if (image != null) //&& image.ContentLength > 0
                {
                    string file_name = Path.GetFileName(image.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/images/restaurants"), file_name);
                    image.SaveAs(path);
                    restaurant.Image = file_name;
                    string fullPath = Request.MapPath("~/Contents/images/restaurants" + oldfilePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    //db.Entry(post.Image).State = EntityState.Modified;
                }
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            db.Restaurants.Remove(restaurant);
            db.SaveChanges();
            string fullPath = Request.MapPath("~/Content/images/restaurants/" + restaurant.Image);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            return RedirectToAction("Index");
        }

        // POST: Restaurants/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Restaurant restaurant = db.Restaurants.Find(id);
        //    db.Restaurants.Remove(restaurant);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
