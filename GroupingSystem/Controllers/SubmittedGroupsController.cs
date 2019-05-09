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

        [Authorize(Roles = "Admin")]
        // GET: SubmittedGroups
        public async Task<ActionResult> Index(bool? filterOut)
        {


            if (filterOut == true)
                    {
                     return View(db.SubmittedGroups.Where(x => x.Approved == false).Where(x => x.Denied == false));
                    }
                    else
                    {
                    return View(await db.SubmittedGroups.ToListAsync());
               }

        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        // GET: SubmittedGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

            int? groupIdFind = db.Groups.Where(x => x.Id == submittedGroup.GroupID).SingleOrDefault()?.Id;          
            Group foundGroup = await db.Groups.FindAsync(groupIdFind);
            ViewBag.chosenGroup = foundGroup;
            return View(submittedGroup);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve([Bind(Include = "Id,GroupID,Approved,Denied,groupOwner,groupEvent,eventId")] SubmittedGroup groupSubmitted)
        {
            if (ModelState.IsValid)
            {


                int? eventIdFind = db.Events.Where(x => x.Id == groupSubmitted.eventId).SingleOrDefault()?.Id;

                Event foundEvent = await db.Events.FindAsync(eventIdFind);

                int? groupIdFind = db.Groups.Where(x => x.Id == groupSubmitted.GroupID).SingleOrDefault()?.Id;

                Group foundGroup = await db.Groups.FindAsync(groupIdFind);



                int available = foundEvent.Tickets_available;
                int deduction = 0;


                if(foundGroup.groupOwner != null)
                {
                    var message = new Message
                    {
                    User = foundGroup.groupOwner,
                    Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been approved",
                    Seen = false,
                    Subject = "Group approved",
                    From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    deduction = 1;
                }
                if (foundGroup.member1 != null)
                {
                    var message = new Message
                    {
                        User = foundGroup.member1,
                        Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been approved",
                        Seen = false,
                        Subject = "Group approved",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    deduction = 2;
                }
                if (foundGroup.member2 != null)
                {
                    var message = new Message
                    {
                        User = foundGroup.member2,
                        Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been approved",
                        Seen = false,
                        Subject = "Group approved",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    deduction = 3;
                }
                if (foundGroup.member3 != null)
                {
                    var message = new Message
                    {
                        User = foundGroup.member3,
                        Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been approved",
                        Seen = false,
                        Subject = "Group approved",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    deduction = 4;
                }
                if (foundGroup.member4 != null)
                {
                    var message = new Message
                    {
                        User = foundGroup.member4,
                        Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been approved",
                        Seen = false,
                        Subject = "Group approved",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                    deduction = 5;
                }

                foundEvent.Tickets_available = available - deduction;
                groupSubmitted.Approved = true;
                db.Entry(groupSubmitted).State = EntityState.Modified;
                db.Entry(foundEvent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                int remainingCheck = foundEvent.Tickets_available;

                if(remainingCheck <= 5)
                {
                    var eventGroups = from g in db.Groups where g.eventId == foundEvent.Id select g.Id;

                    foreach(var grp in eventGroups)
                    {
                        int? findGroupToCheck = db.Groups.Where(x => x.Id == groupSubmitted.GroupID).SingleOrDefault()?.Id;

                        Group groupFound = await db.Groups.FindAsync(findGroupToCheck);
                        if (groupFound.groupSize > remainingCheck)
                        {
                            var purgedGroup = new SubmittedGroup
                            {
                                GroupID = groupFound.Id,
                                Approved = false,
                                Denied = true, 
                                groupOwner = foundGroup.groupOwner,
                                groupEvent = foundGroup.groupEvent,
                                eventId = foundGroup.eventId
                             };
                            groupFound.submitted = true;
                            db.SubmittedGroups.Add(purgedGroup);
                            db.Entry(groupFound).State = EntityState.Modified;
                            

                        }
                        if (groupFound.groupOwner != null)
                        {
                            var message = new Message
                            {
                                User = groupFound.groupOwner,
                                Message1 = "Hi " + groupFound.groupOwner + ", " + "Your group " + groupFound.groupName + " for event " + groupFound.groupEvent + " has been closed due to there not being enough tickets to provide to a group of your selected size.",
                                Seen = false,
                                Subject = "Group too large",
                                From = "Admin",
                                Time = DateTime.Now
                            };
                            db.Messages.Add(message);
                        }
                        if (groupFound.member1 != null)
                        {
                            var message = new Message
                            {
                                User = groupFound.member1,
                                Message1 = "Hi " + groupFound.groupOwner + ", " + "Your group " + groupFound.groupName + " for event " + groupFound.groupEvent + " has been closed due to there not being enough tickets to provide to a group of your selected size.",
                                Seen = false,
                                Subject = "Group too large",
                                From = "Admin",
                                Time = DateTime.Now
                            };
                            db.Messages.Add(message);
                        }
                        if (groupFound.member2 != null)
                        {
                            var message = new Message
                            {
                                User = groupFound.member2,
                                Message1 = "Hi " + groupFound.groupOwner + ", " + "Your group " + groupFound.groupName + " for event " + groupFound.groupEvent + " has been closed due to there not being enough tickets to provide to a group of your selected size.",
                                Seen = false,
                                Subject = "Group too large",
                                From = "Admin",
                                Time = DateTime.Now
                            };
                            db.Messages.Add(message);
                        }
                        if (groupFound.member3 != null)
                        {
                            var message = new Message
                            {
                                User = groupFound.member3,
                                Message1 = "Hi " + groupFound.groupOwner + ", " + "Your group " + groupFound.groupName + " for event " + groupFound.groupEvent + " has been closed due to there not being enough tickets to provide to a group of your selected size.",
                                Seen = false,
                                Subject = "Group too large",
                                From = "Admin",
                                Time = DateTime.Now
                            };
                            db.Messages.Add(message);
                        }
                        if (groupFound.member4 != null)
                        {
                            var message = new Message
                            {
                                User = groupFound.member4,
                                Message1 = "Hi " + groupFound.groupOwner + ", " + "Your group " + groupFound.groupName + " for event " + groupFound.groupEvent + " has been closed due to there not being enough tickets to provide to a group of your selected size.",
                                Seen = false,
                                Subject = "Group too large",
                                From = "Admin",
                                Time = DateTime.Now
                            };
                            db.Messages.Add(message);
                        }
                    }
                }
                await db.SaveChangesAsync();


                return RedirectToAction("Index");
            }
            return View(groupSubmitted);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deny([Bind(Include = "Id,GroupID,Approved,Denied,groupOwner,groupEvent,eventId")] SubmittedGroup groupSubmitted)
        {
            if (ModelState.IsValid)
            {
                int? eventIdFind = db.Events.Where(x => x.Id == groupSubmitted.eventId).SingleOrDefault()?.Id;

                Event foundEvent = await db.Events.FindAsync(eventIdFind);

                int? groupIdFind = db.Groups.Where(x => x.Id == groupSubmitted.GroupID).SingleOrDefault()?.Id;

                Group foundGroup = await db.Groups.FindAsync(groupIdFind);


                if (foundGroup.groupOwner != null)
                {
                    var message = new Message
                    {
                        User = foundGroup.groupOwner,
                        Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been denied.",
                        Seen = false,
                        Subject = "Group denied",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                }
                if (foundGroup.member1 != null)
                {
                    var message = new Message
                    {
                        User = foundGroup.member1,
                        Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been denied",
                        Seen = false,
                        Subject = "Group denied",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);
                }
                if (foundGroup.member2 != null)
                {
                    var message = new Message
                    {
                        User = foundGroup.member2,
                        Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been denied.",
                        Seen = false,
                        Subject = "Group denied",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);

                }
                if (foundGroup.member3 != null)
                {
                    var message = new Message
                    {
                        User = foundGroup.member3,
                        Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been denied",
                        Seen = false,
                        Subject = "Group denied",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);

                }
                if (foundGroup.member4 != null)
                {
                    var message = new Message
                    {
                        User = foundGroup.member4,
                        Message1 = "Hi " + foundGroup.groupOwner + ", " + "Your group " + foundGroup.groupName + " for event " + foundGroup.groupEvent + " has been denied",
                        Seen = false,
                        Subject = "Group denied",
                        From = "Admin",
                        Time = DateTime.Now
                    };
                    db.Messages.Add(message);

                }

                groupSubmitted.Denied = true;
                db.Entry(groupSubmitted).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(groupSubmitted);
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
