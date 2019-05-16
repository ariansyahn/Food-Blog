using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Restopedia.Models;

namespace Restopedia.Controllers
{
    public class PostsController : Controller
    {
        private RestopediaEntities db = new RestopediaEntities();
        
        // GET: Posts
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["RoleId"]) == 1)
            {
                var posts = db.Posts.Include(p => p.User);
                return View(posts.ToList());
            }
            else if (Convert.ToInt32(Session["RoleId"]) == 2)
            {
                return RedirectToAction("Posts");
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
           
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (Convert.ToInt32(Session["RoleId"]) == 2 || Session["RoleId"] == null)
            {
                return RedirectToAction("Login","Users");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Post post = db.Posts.Find(id);
                if (post == null)
                {
                    return HttpNotFound();
                }
                return View(post);
            }
            
        }

        public ActionResult Single(int? id)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Post post = db.Posts.Find(id);
                if (post == null)
                {
                    return HttpNotFound();
                }
          
                return View(post);
        }

        public ActionResult Posts()
        {
            return View(db.Posts.OrderByDescending(Model => Model.Date).ToList());
        }


        // GET: Posts/Create
        public ActionResult Create()
        {
            if (Convert.ToInt32(Session["RoleId"]) == 2 || Session["RoleId"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                //ViewBag.UserId = new SelectList(db.Users, "UserId", "Username");
                return View();
            }
           
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,Title,Text,Image,Date,UserId")] Post post, HttpPostedFileBase image)
        {

            if (ModelState.IsValid)
            {
                string file_name = Path.GetFileName(image.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/posts"), file_name);
                image.SaveAs(path);

                //DateTime test = DateTime.Now;
                //post.Date = test.ToString("yyyy-MM-dd");

                post.Date = Convert.ToDateTime(DateTime.Now);
                // image_path is nvarchar type db column. We save the name of the file in that column. 
                post.Image = file_name;
                post.UserId = Convert.ToInt32(Session["UserId"]);
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", post.UserId);
            return View(post);
        }

        // GET: Posts/Edit/5
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
                Post post = db.Posts.Find(id);
                if (post == null)
                {
                    return HttpNotFound();
                }
                //ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", post.UserId);
                return View(post);
            }
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post, HttpPostedFileBase image)
        { 
            if (ModelState.IsValid)
            {
                var model = post;
                string oldfilePath = model.Image;

                if (image != null) //&& image.ContentLength > 0
                {
                    string file_name = Path.GetFileName(image.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/images/posts"), file_name);
                    image.SaveAs(path);
                    post.Image = file_name;
                    string fullPath = Request.MapPath("~/Contents/images/posts" + oldfilePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    //db.Entry(post.Image).State = EntityState.Modified;
                }
                //else
                //{
                    //db.Posts.Attach(model);
                    //post.Image = post.Image;
                    //db.Entry(post).Property(x => x.Image).IsModified = false;
                    //post.Image = model.Image;
                //}
                //post.UserId = model.UserId;
                post.Date = Convert.ToDateTime(DateTime.Now);
                //post.UserId = model.UserId;
                //db.Entry(post.UserId).State = EntityState.Unchanged;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", post.UserId);            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Username", post.UserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            db.Posts.Remove(post);
            db.SaveChanges();
            string fullPath = Request.MapPath("~/Content/images/posts/" + post.Image);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
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
