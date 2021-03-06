﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntroToMVC.Models;
using Facebook;

namespace IntroToMVC.Controllers
{
    [RequireHttps]
    public class PostsController : Controller
    {
        private FanDBContext db = new FanDBContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        [HttpPost]
        public ActionResult Index(string SearchString)
        {
            var posts = from p in db.Posts
                       select p;

            if (!String.IsNullOrEmpty(SearchString))
            {
                posts = posts.Where(p => p.Content.Contains(SearchString));
            }

            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
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

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Writer,WebSite,PostingDate,Content,Image,Video")] Post post, bool ImageCheckBox, bool VideoCheckBox)
        {
            post.Comments = new List<Comment>();

            if (ModelState.IsValid)
            {
                if (!ImageCheckBox)
                    post.Image = string.Empty;
                if (!VideoCheckBox)
                    post.Video = string.Empty;
                db.Posts.Add(post);
                db.SaveChanges();

                if (FanClubController.access_token != "")
                {
                    var client = new FacebookClient(FanClubController.access_token);

                    client.Post("/me/feed/", new { message = string.Format("Title is:{0} And Writer is:{1}", post.Title, post.Writer) });
                }

                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Writer,WebSite,Content,Image,Video")] Post post, bool ImageCheckBox, bool VideoCheckBox)
        {
            post.PostingDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (!ImageCheckBox)
                    post.Image = string.Empty;
                if (!VideoCheckBox)
                    post.Video = string.Empty;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ManageComments(int? id)
        {
            var comments =
                   from c in db.Comments.ToList()
                   where c.RelatedPostID == id
                   select c;

            TempData["RelatedPostID"] = id;

            return RedirectToAction("Index", "Comments");
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
