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
    public class ForumCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ForumCategories
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await db.ForumCategories.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: ForumCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumCategory forumCategory = await db.ForumCategories.FindAsync(id);
            if (forumCategory == null)
            {
                return HttpNotFound();
            }
            return View(forumCategory);
        }

        [Authorize(Roles = "Admin")]
        // GET: ForumCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: ForumCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Category,CategoryDescription")] ForumCategory forumCategory)
        {
            if (ModelState.IsValid)
            {
                db.ForumCategories.Add(forumCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(forumCategory);
        }

        [Authorize(Roles = "Admin")]
        // GET: ForumCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumCategory forumCategory = await db.ForumCategories.FindAsync(id);
            if (forumCategory == null)
            {
                return HttpNotFound();
            }
            return View(forumCategory);
        }

        [Authorize(Roles = "Admin")]
        // POST: ForumCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Category,CategoryDescription")] ForumCategory forumCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forumCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(forumCategory);
        }

        [Authorize(Roles = "Admin")]
        // GET: ForumCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumCategory forumCategory = await db.ForumCategories.FindAsync(id);
            if (forumCategory == null)
            {
                return HttpNotFound();
            }
            return View(forumCategory);
        }

        [Authorize(Roles = "Admin")]
        // POST: ForumCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ForumCategory forumCategory = await db.ForumCategories.FindAsync(id);
            db.ForumCategories.Remove(forumCategory);
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
