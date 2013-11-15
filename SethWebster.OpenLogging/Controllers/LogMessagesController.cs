using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SethWebster.OpenLogging.Controllers
{
    public class LogMessagesController : Controller
    {
        //
        // GET: /LogMessages/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}