using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntroToMVC.Models;
using System.Collections;
using System.IO;
using Facebook;

namespace IntroToMVC.Controllers
{
    [RequireHttps]
    public class FanClubController : Controller
    {
        public enum Status
        {
            Off,
            Logged,
            Admin
        };

        private FanDBContext db = new FanDBContext();
        private static string message = "OK";
        private static Status status = Status.Off;
        public static string access_token = "";

        public Status getStatus()
        {
            return status;
        }

        public ActionResult Login()
        {
            ViewBag.HtmlStr = message;
            return View();
        }
        public ActionResult LogInFaceBook()
        {
            string app_id = "838250792939146";
            string app_secret = "5e6a472a233200ee63876f238adce751";
            string scope = "publish_actions,manage_pages,user_birthday";

            if (Request["code"] == null)
            {
                Response.Redirect(string.Format(
                    "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}",
                    app_id, Request.Url.AbsoluteUri, scope));
            }
            else
            {
                Dictionary<string, string> tokens = new Dictionary<string, string>();
                string url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}",
                    app_id, Request.Url.AbsoluteUri, scope, Request["code"].ToString(), app_secret);

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string vals = reader.ReadToEnd();
                    foreach(string token in vals.Split('&'))
                    {
                        tokens.Add(token.Substring(0, token.IndexOf("=")),
                                   token.Substring(token.IndexOf("=") + 1, token.Length - token.IndexOf("=") - 1));
                    }
                }

                access_token = tokens["access_token"];
            }

            if (access_token != "")
            {
                status = Status.Admin;
                this.Session["isAdmin"] = "Yes";
                dynamic me;
                Fan NewFan;

                var fans = from f in db.Fans
                           select f;
                
                var client = new FacebookClient(FanClubController.access_token);
                try
                {
                    me = client.Get("me", new { fields = "name,id,first_name,last_name,gender" });
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "FanClub");
                }

                string username = me.first_name;
                string password = me.last_name;

                fans = fans.Where(s => s.UserName.Equals(username) &&
                                       s.Password.Equals(password));
                if (fans.ToList().Count != 0)
                    this.Session["userID"] = fans.ToList()[0].ID;

                if (this.Session["userID"] == null)
                {
                    NewFan = new Fan(2,
                                     me.first_name,
                                     me.last_name,
                                     me.gender,
                                     DateTime.Today,
                                     1,
                                     me.first_name,
                                     me.last_name
                                     );

                    db.Fans.Add(NewFan);
                    db.SaveChanges();

                    this.Session["userID"] = NewFan.ID;
                }

                return RedirectToAction("Index", "Blog");
            }
            else
            {
                message = "Incorrect details";
                return RedirectToAction("Login", "FanClub"); 
            }
        }
        public ActionResult VerifyUser(string userName, string password)
        {
            var fans = from f in db.Fans
                       select f;

            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                
                fans = fans.Where(s => s.UserName.Equals(userName) &&
                                       s.Password.Equals(password));
            }


            if (fans.ToList().Count == 1)
            {
                if (fans.ToList()[0].Permission == 2)
                {
                    status = Status.Admin;
                    this.Session["isAdmin"] = "Yes";
                }
                else
                {
                    status = Status.Logged;
                    this.Session["isAdmin"] = "No";
                }

                this.Session["userID"] = fans.ToList()[0].ID;
                return RedirectToAction("Index", "Blog");
            }
            else
            {
                message = "Incorrect details";
                return RedirectToAction("Login", "FanClub");
            }
        }

        // GET: FanClub
        public ActionResult Index()
        {
            return View(db.Fans.ToList());
        }

        // GET: FanClub/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // GET: FanClub/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult IndexAdmin()
        {
            if (status != Status.Admin)
            {
                return null;
            }

            return View(db.Fans.ToList());
        }

        // POST: FanClub/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,UserName,Gender,BirthDate,NumOfYearsInClub,Password")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                db.Fans.Add(fan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fan);
        }

        // GET: FanClub/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // POST: FanClub/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,UserName,Gender,BirthDate,NumOfYearsInClub,Password")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fan);
        }

        // GET: FanClub/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // POST: FanClub/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fan fan = db.Fans.Find(id);
            db.Fans.Remove(fan);
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

        public ActionResult BranchesReport()
        {
            var AdminOfBranch = from branch in db.Branches
                                join fan in db.Fans on branch.Manager.ID equals fan.ID
                                select new BranchReport{ Manager = fan, Branch = branch };

            return View(AdminOfBranch.ToList());
        }

        public ActionResult BranchesSummery()
        {
            var AdminOfBranch = from branch in db.Branches
                                join fan in db.Fans on branch.Manager.ID equals fan.ID
                                group branch by fan.ID
                                into grp
                                    select new BranchSummery { FirstName = (from f in db.Fans
                                                                           where f.ID == grp.Key
                                                                           select f.FirstName).FirstOrDefault(), Count = grp.Select(x => x.ID).Count() };

            return View(AdminOfBranch.ToList());
        }
    }
}