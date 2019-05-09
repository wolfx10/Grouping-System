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
using System.Diagnostics;

namespace GroupingSystem.Controllers
{
    public class UserProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserProfiles
        public async Task<ActionResult> Index()
        {
            return View(await db.UserProfiles.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: UserProfiles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = await db.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }


            return View(userProfile);
        }

        [Authorize]
        public async Task<ActionResult> viewProfile(string id)
        {
  
           int? idFind = db.UserProfiles.Where(x => x.username == id).SingleOrDefault()?.Id;

           UserProfile foundProfile = await db.UserProfiles.FindAsync(idFind);
           
            var findGroups = (from g in db.Groups
                              where g.groupOwner == foundProfile.username || g.member1 == foundProfile.username || g.member2 == foundProfile.username || g.member3 == foundProfile.username || g.member4 == foundProfile.username
                              select g);
            var findApprovedGroups = (from a in db.SubmittedGroups
                                      where a.Approved == true
                                      select a);

            var listOfAttendedEvents = new List<Group>();

            foreach (var a in findApprovedGroups)
            {
                foreach(var g in findGroups)
                {
                    if(a.GroupID == g.Id)
                    {
                        listOfAttendedEvents.Add(g);
                    }
                }

            }
            
            ViewBag.eventsAttended = listOfAttendedEvents.Count;

            var recentEvents = listOfAttendedEvents.Skip(Math.Max(0, listOfAttendedEvents.Count() - 5));

            ViewBag.groupsAttended = recentEvents;

            return View(foundProfile);

        }

        [Authorize(Roles = "Admin")]
        // GET: UserProfiles/Create
        public ActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        // POST: UserProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,firstName,lastName,email,DoB,username")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                db.UserProfiles.Add(userProfile);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        [Authorize(Roles = "Admin")]
        // GET: UserProfiles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = await db.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        [Authorize(Roles = "Admin")]
        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,firstName,lastName,email,DoB,username")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userProfile).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userProfile);
        }

        [Authorize(Roles = "Admin")]
        // GET: UserProfiles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = await db.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        [Authorize(Roles = "Admin")]
        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserProfile userProfile = await db.UserProfiles.FindAsync(id);
            db.UserProfiles.Remove(userProfile);
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
