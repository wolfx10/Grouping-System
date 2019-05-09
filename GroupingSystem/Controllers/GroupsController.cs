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

        [Authorize]
        // GET: Groups
        public async Task<ActionResult> Index(int? evSearch, bool? myGroups)
        {
            var eventList = from e in db.Events
                            select e;


            var events = from e in db.Events
                         where (e.Tickets_available > 0)
                         select e;

            var filtEvents = from v in events
                             where v.Date > DateTime.Now
                             select v;

            ViewBag.eventsList = await eventList.ToListAsync();

            ViewBag.Events = new SelectList(filtEvents, "Id", "eventAndTicketsAndDate");

            if (evSearch != null)
            {
                if(myGroups == true)
                {
                    ViewBag.searchResult = "Currently searching for your groups for event" + evSearch;
                    return View(db.Groups.Where(x => x.eventId == evSearch).Where(x => new[] { x.groupOwner, x.member1, x.member2, x.member3, x.member4 }.Any(s => s.Contains(User.Identity.Name))));
                }
                else
                {
                    ViewBag.searchResult = "Currently searching for groups for event" + evSearch;
                    return View(db.Groups.Where(x => x.eventId == evSearch));
                }
            }

            if (evSearch == null)
            {
                if(myGroups == true)
                {
                    ViewBag.searchResult = "Currently searching for your groups" + evSearch;
                    return View(db.Groups.Where(x => new[] { x.groupOwner, x.member1, x.member2, x.member3, x.member4 }.Any(s => s.Contains(User.Identity.Name))));
                }
                else
                {
                ViewBag.searchResult = "Currently showing all groups";
                return View(await db.Groups.ToListAsync());
                }
            }

            ViewBag.searchResult = "Currently showing all groups";



            return View(await db.Groups.ToListAsync());

        }


        [Authorize]
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

        [Authorize]
        // GET: Groups/Create
        public ActionResult Create()
        {

            var events = from e in db.Events
                         where (e.Tickets_available > 0)
                         select e;

            var filtEvents = from v in events
                             where v.Date > DateTime.Now
                             select v;

            ViewBag.Events = new SelectList(filtEvents, "Name", "eventAndTicketsAndDate");
            return View();
        }


        [Authorize]
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitGroup([Bind(Include = "Id,groupName,groupSize,groupOwner,member1,member2,member3,member4,submitted,groupDescription,groupEvent,eventId")]  Group subGroup)
        {


            if (subGroup.groupOwner != null)
            {
                var message = new Message
                {
                    User = subGroup.groupOwner,
                    Message1 = "Hi " + subGroup.groupOwner + ", " + "Your group " + subGroup.groupName + " for event " + subGroup.groupEvent + " has been submitted for approval",
                    Seen = false,
                    Subject = "Group submitted",
                    From = "Admin",
                    Time = DateTime.Now
            };
                db.Messages.Add(message);
            }
            if (subGroup.member1 != null)
            {
                var message = new Message
                {
                    User = subGroup.member1,
                    Message1 = "Hi " + subGroup.groupOwner + ", " + "Your group " + subGroup.groupName + " for event " + subGroup.groupEvent + " has been submitted for approval",
                    Seen = false,
                    Subject = "Group submitted",
                    From = "Admin",
                    Time = DateTime.Now
                };
                db.Messages.Add(message);
            }
            if (subGroup.member2 != null)
            {
                var message = new Message
                {
                    User = subGroup.member2,
                    Message1 = "Hi " + subGroup.groupOwner + ", " + "Your group " + subGroup.groupName + " for event " + subGroup.groupEvent + " has been submitted for approval",
                    Seen = false,
                    Subject = "Group submitted",
                    From = "Admin",
                    Time = DateTime.Now
                };
                db.Messages.Add(message);
            }
            if (subGroup.member3 != null)
            {
                var message = new Message
                {
                    User = subGroup.member3,
                    Message1 = "Hi " + subGroup.groupOwner + ", " + "Your group " + subGroup.groupName + " for event " + subGroup.groupEvent + " has been submitted for approval",
                    Seen = false,
                    Subject = "Group submitted",
                    From = "Admin",
                    Time = DateTime.Now
                };
                db.Messages.Add(message);
            }
            if (subGroup.member4 != null)
            {
                var message = new Message
                {
                    User = subGroup.member4,
                    Message1 = "Hi " + subGroup.groupOwner + ", " + "Your group " + subGroup.groupName + " for event " + subGroup.groupEvent + " has been submitted for approval",
                    Seen = false,
                    Subject = "Group submitted",
                    From = "Admin",
                    Time = DateTime.Now
                };
                db.Messages.Add(message);
            }


            var submitted = new SubmittedGroup
            {
                GroupID = subGroup.Id,
                groupOwner = subGroup.groupOwner,
                groupEvent = subGroup.groupEvent,
                eventId = subGroup.eventId
            };

            if (ModelState.IsValid)
            {
                subGroup.submitted = true;
                db.Entry(subGroup).State = EntityState.Modified;
                db.SubmittedGroups.Add(submitted);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subGroup);
        }






        [Authorize]
        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "groupName,groupDescription,groupSize,groupOwner,groupEvent, eventId")] Group newGroup)
        {
            Event eventChecked = new Event();

            var events = from e in db.Events
                         where (e.Tickets_available > 0)
                         select e;

            var filtEvents = from v in events
                             where v.Date > DateTime.Now
                             select v;

            ViewBag.Events = new SelectList(filtEvents, "Name", "eventAndTicketsAndDate");


            if (ModelState.IsValid)
            {
                foreach (Event dbEvent in db.Events)
                {
                    if (newGroup.groupEvent == dbEvent.Name)
                    {
                        eventChecked = dbEvent;
                        break;
                    }

                }

                if (newGroup.groupEvent == null)
                {
                    ModelState.AddModelError("groupEvent", "Please pick an event.");
                    return View(newGroup);
                }

                if (newGroup.groupSize > eventChecked.Tickets_available)
                {
                    ModelState.AddModelError("groupSize", "There are not enough tickets for  group this size.");
                    return View(newGroup);
                }

                if (newGroup.groupSize < 0)
                {
                    ModelState.AddModelError("groupSize", "Can't have a negative group size.");
                    return View(newGroup);
                }

                var findEventID = (from x in db.Events
                                     where x.Name == newGroup.groupEvent
                                     select x.Id).SingleOrDefault();

                int? idFind = db.Events.Where(x => x.Name == newGroup.groupEvent).SingleOrDefault()?.Id;

                Event foundEvent = await db.Events.FindAsync(idFind);

                newGroup.eventId = foundEvent.Id;
                newGroup.groupOwner = User.Identity.Name;

                    db.Groups.Add(newGroup);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");

            }

            return View(newGroup);
        }

        [Authorize]
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Join([Bind(Include = "Id,groupName,groupSize,groupOwner,member1,member2,member3,member4,submitted,groupDescription,groupEvent,eventId")] Group group)
        {

                if (ModelState.IsValid){

                if (group.member1 == null)
                {
                    group.member1 = User.Identity.Name;
                    var message = new Message
                    {
                        User = group.member1,
                        Message1 = "Hi " + group.groupOwner + ", " + User.Identity.Name + " has joined your group " + group.groupName + " for event " + group.groupEvent,
                        Seen = false,
                        Subject = "New Group Member",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Entry(group).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                if (group.member2 == null)
                {
                    group.member2 = User.Identity.Name;
                    db.Entry(group).State = EntityState.Modified;
                    var message = new Message
                    {
                        User = group.groupOwner,
                        Message1 = "Hi " + group.groupOwner + ", " + User.Identity.Name + " has joined your group " + group.groupName + " for event " + group.groupEvent,
                        Seen = false,
                        Subject = "New Group Member",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                if (group.member3 == null)
                {
                    group.member3 = User.Identity.Name;
                    db.Entry(group).State = EntityState.Modified;
                    var message = new Message
                    {
                        User = group.groupOwner,
                        Message1 = "Hi " + group.groupOwner + ", " + User.Identity.Name + " has joined your group " + group.groupName + " for event " + group.groupEvent,
                        Seen = false,
                        Subject = "New Group Member",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                if (group.member4 == null)
                {
                    group.member4 = User.Identity.Name;
                    db.Entry(group).State = EntityState.Modified;
                    var message = new Message
                    {
                        User = group.groupOwner,
                        Message1 = "Hi " + group.groupOwner + ", " + User.Identity.Name + " has joined your group " + group.groupName + " for event " + group.groupEvent,
                        Seen = false,
                        Subject = "New Group Member",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            }
            return View(group);
        }

        [Authorize]
        // GET: Groups/Join
        public async Task<ActionResult> Leave(int? id)
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Leave([Bind(Include = "Id,groupName,groupSize,groupOwner,member1,member2,member3,member4,submitted,groupDescription,groupEvent,eventId")] Group group)
        {

            if (ModelState.IsValid)
            {

                if (group.member1 == User.Identity.Name)
                {
                    group.member1 = null;
                    db.Entry(group).State = EntityState.Modified;
                    var message = new Message
                    {
                        User = group.groupOwner,
                        Message1 = "Hi " + group.groupOwner + ", " + User.Identity.Name + " has left your group " + group.groupName + " for event " + group.groupEvent,
                        Seen = false,
                        Subject = "New Group Member",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                if (group.member2 == User.Identity.Name)
                {
                    group.member2 = null;
                    db.Entry(group).State = EntityState.Modified;
                    var message = new Message
                    {
                        User = group.groupOwner,
                        Message1 = "Hi " + group.groupOwner + ", " + User.Identity.Name + " has left your group " + group.groupName + " for event " + group.groupEvent,
                        Seen = false,
                        Subject = "New Group Member",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                if (group.member3 == User.Identity.Name)
                {
                    group.member3 = null;
                    db.Entry(group).State = EntityState.Modified;
                    var message = new Message
                    {
                        User = group.groupOwner,
                        Message1 = "Hi " + group.groupOwner + ", " + User.Identity.Name + " has left your group " + group.groupName + " for event " + group.groupEvent,
                        Seen = false,
                        Subject = "New Group Member",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                if (group.member4 == User.Identity.Name)
                {
                    group.member4 = null;
                    db.Entry(group).State = EntityState.Modified;
                    var message = new Message
                    {
                        User = group.groupOwner,
                        Message1 = "Hi " + group.groupOwner + ", " + User.Identity.Name + " has left your group " + group.groupName + " for event " + group.groupEvent,
                        Seen = false,
                        Subject = "New Group Member",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            }
            return View(group);
        }






        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,groupName,groupSize,groupOwner,member1,member2,member3,member4,submitted,groupDescription,groupEvent,eventId")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [Authorize]
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

        [Authorize]
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



        [Authorize]
        // GET: Groups/Create
        public async Task<ActionResult> CreateFromEvent(int id)
        {

            int? idFind = db.Events.Where(x => x.Id == id).SingleOrDefault()?.Id;

            Event foundEvent = await db.Events.FindAsync(idFind);

            var events = from e in db.Events
                         where (e.Tickets_available > 0)
                         select e;

            var filtEvents = from v in events
                             where v.Date > DateTime.Now
                             select v;

            ViewBag.PickedEvent = foundEvent;
            ViewBag.Events = new SelectList(filtEvents, "Name", "eventAndTicketsAndDate");

            return View();
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateFromEvent([Bind(Include = "groupName,groupDescription,groupSize,groupOwner,groupEvent, eventId")] Group newGroup)
        {
            Event eventChecked = new Event();

            var events = from e in db.Events
                         where (e.Tickets_available > 0)
                         select e;

            var filtEvents = from v in events
                             where v.Date > DateTime.Now
                             select v;

            ViewBag.Events = new SelectList(filtEvents, "Name", "eventAndTicketsAndDate");


            if (ModelState.IsValid)
            {
                foreach (Event dbEvent in db.Events)
                {
                    if (newGroup.groupEvent == dbEvent.Name)
                    {
                        eventChecked = dbEvent;
                        break;
                    }

                }

                if (newGroup.groupEvent == null)
                {
                    ModelState.AddModelError("groupEvent", "Please pick an event.");
                    return View(newGroup);
                }

                if (newGroup.groupSize > eventChecked.Tickets_available)
                {
                    ModelState.AddModelError("groupSize", "There are not enough tickets for  group this size.");
                    return View(newGroup);
                }

                if (newGroup.groupSize < 0)
                {
                    ModelState.AddModelError("groupSize", "Can't have a negative group size.");
                    return View(newGroup);
                }

                var findEventID = (from x in db.Events
                                   where x.Name == newGroup.groupEvent
                                   select x.Id).SingleOrDefault();

                int? idFind = db.Events.Where(x => x.Name == newGroup.groupEvent).SingleOrDefault()?.Id;

                Event foundEvent = await db.Events.FindAsync(idFind);

                newGroup.eventId = foundEvent.Id;
                newGroup.groupOwner = User.Identity.Name;

                db.Groups.Add(newGroup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            return View(newGroup);
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
