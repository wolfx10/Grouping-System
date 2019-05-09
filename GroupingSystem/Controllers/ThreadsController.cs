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

namespace GroupingSystem.Controllers
{
    public class ThreadsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        // GET: Threads
        public async Task<ActionResult> Index()
        {
            return View(await db.Threads.ToListAsync());
        }


        [Authorize]
        public async Task<ActionResult> ViewThreads(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var categoryName = from c in db.ForumCategories
                               where c.Id == id
                               select c;

            var validThreads = (from t in db.Threads
                                 where t.category == id
                                 select t).OrderByDescending(t => t.LastUpdated);


            ViewBag.category = await categoryName.ToListAsync();
            ViewBag.categoryId = id;

            return View(await validThreads.ToListAsync());
        }


        [Authorize(Roles = "Admin")]
        // GET: Threads/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thread thread = await db.Threads.FindAsync(id);
            if (thread == null)
            {
                return HttpNotFound();
            }
            return View(thread);
        }

        [Authorize]
        // GET: Threads/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        // POST: Threads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,threadTitle,createdBy,category, OP, LastUpdated")] Thread thread, int category)
        {
            thread.createdBy = User.Identity.Name;
            thread.category = category;
            thread.LastUpdated = DateTime.Now;
            if (ModelState.IsValid)
            {
                string page = "ViewThreads/" + thread.category;
                db.Threads.Add(thread);
                await db.SaveChangesAsync();
                return RedirectToAction(page);
            }

            return View(thread);
        }

        [Authorize(Roles = "Admin")]
        // GET: Threads/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thread thread = await db.Threads.FindAsync(id);
            if (thread == null)
            {
                return HttpNotFound();
            }
            return View(thread);
        }

        [Authorize(Roles = "Admin")]
        // POST: Threads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,threadTitle,createdBy")] Thread thread)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thread).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(thread);
        }



        [Authorize(Roles = "Admin")]
        // GET: Threads/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thread thread = await db.Threads.FindAsync(id);
            if (thread == null)
            {
                return HttpNotFound();
            }
            return View(thread);
        }

        [Authorize(Roles = "Admin")]
        // POST: Threads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Thread thread = await db.Threads.FindAsync(id);
            int threadCat = thread.category;
            db.Threads.Remove(thread);
            await db.SaveChangesAsync();
            return RedirectToAction("ViewThreads", new { id = threadCat });
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
