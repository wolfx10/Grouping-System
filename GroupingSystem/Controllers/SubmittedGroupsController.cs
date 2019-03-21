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
    public class SubmittedGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SubmittedGroups
        public async Task<ActionResult> Index()
        {
            return View(await db.SubmittedGroups.ToListAsync());
        }

        // GET: SubmittedGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubmittedGroup submittedGroup = await db.SubmittedGroups.FindAsync(id);
            if (submittedGroup == null)
            {
                return HttpNotFound();
            }
            return View(submittedGroup);
        }

        // GET: SubmittedGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubmittedGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,GroupID,Approved,Denied,groupOwner,groupEvent")] SubmittedGroup submittedGroup)
        {
            if (ModelState.IsValid)
            {
                db.SubmittedGroups.Add(submittedGroup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(submittedGroup);
        }

        // GET: SubmittedGroups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubmittedGroup submittedGroup = await db.SubmittedGroups.FindAsync(id);
            if (submittedGroup == null)
            {
                return HttpNotFound();
            }
            return View(submittedGroup);
        }

        // POST: SubmittedGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,GroupID,Approved,Denied,groupOwner,groupEvent")] SubmittedGroup submittedGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(submittedGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(submittedGroup);
        }

        // GET: SubmittedGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubmittedGroup submittedGroup = await db.SubmittedGroups.FindAsync(id);
            if (submittedGroup == null)
            {
                return HttpNotFound();
            }
            return View(submittedGroup);
        }

        // POST: SubmittedGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubmittedGroup submittedGroup = await db.SubmittedGroups.FindAsync(id);
            db.SubmittedGroups.Remove(submittedGroup);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubmittedGroup submittedGroup = await db.SubmittedGroups.FindAsync(id);
            if (submittedGroup == null)
            {
                return HttpNotFound();
            }
            return View(submittedGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve([Bind(Include = "Id,GroupID,Approved,Denied,groupOwner,groupEvent")] SubmittedGroup submittedGroup)
        {
            if (ModelState.IsValid)
            {
                submittedGroup.Approved = true;
                db.Entry(submittedGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(submittedGroup);
        }

        public async Task<ActionResult> Deny(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubmittedGroup submittedGroup = await db.SubmittedGroups.FindAsync(id);
            if (submittedGroup == null)
            {
                return HttpNotFound();
            }
            return View(submittedGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deny([Bind(Include = "Id,GroupID,Approved,Denied,groupOwner,groupEvent")] SubmittedGroup submittedGroup)
        {
            if (ModelState.IsValid)
            {
                submittedGroup.Denied = true;
                db.Entry(submittedGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(submittedGroup);
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
