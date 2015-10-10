using IntroToMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntroToMVC.Controllers
{
    [RequireHttps]
    public class BlogController : Controller
    {
        private FanDBContext db = new FanDBContext();
        private FanClubController fccInstance = new FanClubController();

        private bool isLoggeedIn()
        {
            // Verify that the guest is logged in
            if (fccInstance.getStatus() == IntroToMVC.Controllers.FanClubController.Status.Off)
            {
                this.Session["isAdmin"] = "No";
                return false;
            }
            if (fccInstance.getStatus() == IntroToMVC.Controllers.FanClubController.Status.Admin)
                this.Session["isAdmin"] = "Yes";
            return true;
        }

        // GET: Blog
        public ActionResult Index()
        {
            if (!isLoggeedIn())
                return null;

            List<Post> posts = new List<Post>(db.Posts.ToList());
            foreach (var item in posts)
            {

                var comments = 
                    from c in db.Comments.ToList()
                    where c.RelatedPostID == item.ID
                    select c;

                item.Comments = comments.ToList();
            }

            return View(posts);
        }
        

        [HttpPost]
        public ActionResult Index(string firstName, string lastName, string webSite, string content)
        {
            var posts = from p in db.Posts
                           select p;

            if (!String.IsNullOrEmpty(firstName))
            {
                posts = posts.Where(p => p.Writer.Equals(firstName));
            }

            if (!String.IsNullOrEmpty(lastName))
            {
                posts = posts.Where(p => p.Writer.Equals(lastName));
            }

            if (!String.IsNullOrEmpty(webSite))
            {
                posts = posts.Where(p => p.WebSite.Equals(webSite));
            }

            if (!String.IsNullOrEmpty(content))
            {
                posts = posts.Where(p => p.Content.Contains(content));
            }


            posts.ToList().ForEach(post =>
                {
                    post.Comments = db.Comments.ToList().Where(comment =>
                                                        comment.RelatedPostID == post.ID).ToList();
                });

            return View(posts);
        }

        public ActionResult Contact()
        {
            List<Branch> branches = new List<Branch>(db.Branches.ToList());
            string parameters = "";

            for (int i = 0; i < branches.Count; i++)
            {
                if (i != 0)
                {
                    parameters += "&";
                }
                parameters += "lat" + i.ToString() + "=" + branches[i].lat;
                parameters += "&lng" + i.ToString() + "=" + branches[i].lng;
            }

            ViewBag.HtmlStr = parameters;

            return View();
        }

        public ActionResult AdminPanel()
        {
            if (fccInstance.getStatus() != IntroToMVC.Controllers.FanClubController.Status.Admin)
                return null;

            return View();
        }

        public ActionResult RedirectToFanClub()
        {
            if (!isLoggeedIn())
                return null;

            return RedirectToAction("Index", "FanClub");
        }

        public ActionResult RedirectToPosts()
        {
            if (!isLoggeedIn())
                return null;

            return RedirectToAction("Index", "Posts");
        }

        public ActionResult RedirectToBlog()
        {
            if (!isLoggeedIn())
                return null;

            return RedirectToAction("Index", "Blog");
        }

        public ActionResult RedirectToContact()
        {
            if (!isLoggeedIn())
                return null;

            return RedirectToAction("Contact", "Blog");
        }

        public ActionResult RedirectToAdmin()
        {
            if (fccInstance.getStatus() != IntroToMVC.Controllers.FanClubController.Status.Admin)
            {
                return null;
            }
            else
            {
                return RedirectToAction("AdminPanel", "Blog");
            }
        }

        public ActionResult RedirectToBranchPage()
        {
            if (fccInstance.getStatus() != IntroToMVC.Controllers.FanClubController.Status.Admin)
                return null;

            return RedirectToAction("Index", "Branches");
        }
    }
}