using SethWebster.OpenLogging.Hubs;
using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SethWebster.OpenLogging.Controllers
{
    [Authorize]
    public class LogMessagesController : Controller
    {
        DBContext c = new DBContext();
        //
        // GET: /LogMessages/
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var owner = c.Users.Include("Clients").First(u=>u.UserName==User.Identity.Name);
            return View(owner.Clients);
        }

        public async Task<ActionResult> DoMessage()
        {
            var owner = c.Users.Include("Clients").First(u => u.UserName == User.Identity.Name);
            
            LogMessageSyndication.ReportMessage(new Models.LogMessage()
            {
                Title = "A new Title " + DateTimeOffset.Now.ToString(),
                Body = "Body " + DateTimeOffset.Now.ToString(),
                Category = "ERROR",
                Message = "Here is a message",
                Client = owner.Clients.First()
            });
            return Content("Created");
        }
    }
}