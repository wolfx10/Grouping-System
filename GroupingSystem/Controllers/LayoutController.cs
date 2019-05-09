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
    public class LayoutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Layout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMessageStatus()
        {
            string messageRead = "(0)";
            int totalMessages = 0;

            var userMessages = from m in db.Messages
                               where m.User == User.Identity.Name
                               select m;

            foreach (Message m in userMessages)
            {
                if(m.Seen == false)
                {
                    totalMessages = totalMessages + 1;
                    messageRead = "(" + totalMessages + ")";
                }
            }

            return Content(messageRead);
        }

    }
}