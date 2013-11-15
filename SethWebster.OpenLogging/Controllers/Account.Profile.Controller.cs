using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SethWebster.OpenLogging.Controllers
{
    public partial class AccountController
    {
        DBContext _data = new DBContext();
        [Authorize]
        public async Task<ActionResult> Profile()
        {
            var user = _data.Users.Include("Clients").First(u => u.UserName == User.Identity.Name);
           
            return View(user);
        }
    }
}