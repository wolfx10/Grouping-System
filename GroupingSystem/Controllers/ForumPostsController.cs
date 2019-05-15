using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GroupingSystem.Models;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace GroupingSystem.Controllers
{
    public class ForumPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ForumPosts
        public async Task<ActionResult> Index()
        {
            return View(await db.ForumPosts.ToListAsync());
        }

        //View comments within a specified thread ID
        [Authorize]
        public async Task<ActionResult> ViewComments(int? id, int? category, string title)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var validPosts = from p in db.ForumPosts
                               where p.InThread == id
                               select p;

            var getOP = from o in db.Threads
                        where o.Id == id
                        select o;

            ViewBag.inThread = id;
            ViewBag.threadTitle = title;

            ViewBag.Posts = await validPosts.ToListAsync();
            ViewBag.Category = category;
            ViewBag.Time = DateTime.Now;
            ViewBag.OP = await getOP.ToListAsync();
            return View();
        }

        //Post a comment to the thread and reload the page
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ViewComments([Bind(Include = "Id,PostedBy,Comment,InThread, InCategory, Time")] int? inThread, int? inCategory, ForumPost forumPost)
        {
            forumPost.PostedBy = User.Identity.Name;
            int? Thread = inThread;
            int? Category = inCategory;

            int? idFind = db.Threads.Where(x => x.Id == inThread).SingleOrDefault()?.Id;

            Thread foundThread = await db.Threads.FindAsync(idFind);

            string Title = foundThread.threadTitle;

            if (ModelState.IsValid)
            {
                var time = DateTime.Now;
                forumPost.Time = time;
                db.ForumPosts.Add(forumPost);
                Thread thread = await db.Threads.FindAsync(forumPost.InThread);
                thread.LastUpdated = time;
                db.Entry(thread).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("ViewComments", new {id = Thread, category = Category, title = Title });
            }

            return View(forumPost);
        }



        [Authorize(Roles ="Admin")]
        // GET: ForumPosts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumPost forumPost = await db.ForumPosts.FindAsync(id);
            if (forumPost == null)
            {
                return HttpNotFound();
            }
            return View(forumPost);
        }

        [Authorize(Roles = "Admin")]
        // GET: ForumPosts/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        // POST: ForumPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PostedBy,Comment,InThread")] ForumPost fPost)
        {
            if (ModelState.IsValid)
            {
                db.ForumPosts.Add(fPost);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(fPost);
        }

        [Authorize(Roles = "Admin")]
        // GET: ForumPosts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumPost forumPost = await db.ForumPosts.FindAsync(id);
            if (forumPost == null)
            {
                return HttpNotFound();
            }
            return View(forumPost);
        }

        [Authorize(Roles = "Admin")]
        // POST: ForumPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PostedBy,Comment,InThread")] ForumPost forumPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forumPost).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(forumPost);
        }

        [Authorize(Roles = "Admin")]
        // GET: ForumPosts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumPost forumPost = await db.ForumPosts.FindAsync(id);
            if (forumPost == null)
            {
                return HttpNotFound();
            }
            return View(forumPost);
        }

        [Authorize(Roles = "Admin")]
        // POST: ForumPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ForumPost forumPost = await db.ForumPosts.FindAsync(id);
            db.ForumPosts.Remove(forumPost);
            await db.SaveChangesAsync();
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
