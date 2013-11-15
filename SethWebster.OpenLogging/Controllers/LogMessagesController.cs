using SethWebster.OpenLogging.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SethWebster.OpenLogging.Controllers
{
    public class LogMessagesController : Controller
    {
        //
        // GET: /LogMessages/
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> DoMessage()
        {
            LogMessageSyndication.ReportMessage(new Models.LogMessage()
            {
                Title = "A new Title " + DateTimeOffset.Now.ToString(),
                Body = "Body " + DateTimeOffset.Now.ToString(),
                Category = "ERROR",
                Message = "Here is a message"
            });
            return Content("Created");
        }
    }
}