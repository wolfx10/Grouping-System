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
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public async Task<ActionResult> Index()
        {
            return View(await db.Groups.ToListAsync());
        }


        // GET: Groups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            ViewBag.Events = new SelectList(db.Events, "Name", "eventAndTickets" );
            return View();
        }



        // GET: Groups/SubmitGroup
        public async Task<ActionResult> SubmitGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitGroup([Bind(Include = "Id,groupName,groupSize,groupOwner,member1,member2,member3,member4,submitted,groupDescription,groupEvent")]  Group group)
        {

            var submitted = new SubmittedGroup
            {
                GroupID = group.Id,
                groupOwner = group.groupOwner,
                groupEvent = group.groupEvent
            };


            db.SubmittedGroups.Add(submitted);

            if (ModelState.IsValid)
            {
                group.submitted = true;
                db.Entry(group).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(group);
        }







        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "groupName,groupDescription,groupSize,groupOwner,groupEvent")] Group group)
        {
            Event eventChecked = new Event();
            ViewBag.Events = new SelectList(db.Events, "Name", "eventAndTickets");

            if (ModelState.IsValid)
            {
                foreach (Event dbEvent in db.Events)
                {
                    if (group.groupEvent == dbEvent.Name)
                    {
                        eventChecked = dbEvent;
                        break;
                    }

                }

                if(group.groupSize > eventChecked.Tickets_available)
                {
                    ModelState.AddModelError("groupSize", "There are not enough tickets for  group this size.");
                    return View(group);
                }

                if (group.groupSize < 0)
                {
                    ModelState.AddModelError("groupSize", "Can't have a negative group size.");
                    return View(group);
                }

                    group.groupOwner = User.Identity.Name;
                    db.Groups.Add(group);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");

            }

            return View(group);
        }


        // GET: Groups/Join
        public async Task<ActionResult> Join(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            int memberCount = 0;

            if (group.member1 != null)
            {
                memberCount = 2;
            }

            if (group.member2 != null)
            {
                memberCount = 3;
            }

            if (group.member3 != null)
            {
                memberCount = 4;
            }

            if (group.member4 != null)
            {
                memberCount = 5;
            }

            ViewBag.memberCount = memberCount;
            return View(group);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Join([Bind(Include = "Id,groupName,groupSize,groupOwner,member1,member2,member3,member4,submitted,groupDescription,groupEvent")] Group group)
        {

                if (ModelState.IsValid){

                if (group.member1 == null)
                {
                    group.member1 = User.Identity.Name;
                    db.Entry(group).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                if (group.member2 == null)
                {
                    group.member2 = User.Identity.Name;
                    db.Entry(group).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                if (group.member3 == null)
                {
                    group.member3 = User.Identity.Name;
                    db.Entry(group).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                if (group.member4 == null)
                {
                    group.member4 = User.Identity.Name;
                    db.Entry(group).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            }
            return View(group);
        }









        // GET: Groups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,groupName,groupSize,groupOwner,member1,member2,member3,member4,submitted,groupDescription,groupEvent")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }
       
        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Group group = await db.Groups.FindAsync(id);
            db.Groups.Remove(group);
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
