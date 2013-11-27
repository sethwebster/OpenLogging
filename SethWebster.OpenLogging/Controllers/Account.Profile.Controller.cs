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

        public async Task<ActionResult> ResetApiKey()
        {
            var user = _data.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            user.UserApiKey = Guid.NewGuid();
            await _data.SaveChangesAsync();
            return RedirectToAction("Profile");
        }

        public async Task<ActionResult> ResetClientApiKey(int id)
        {
            var user = _data.Users.Include("Clients").First(u => u.UserName.Equals(User.Identity.Name));
            var client = user.Clients.First(u => u.ClientId.Equals(id));
            client.CurrentApiKey = Guid.NewGuid();
            await _data.SaveChangesAsync();
            return RedirectToAction("Profile");

        }

        public async Task<ActionResult> CreateClient()
        {
            return View(new Client());
        }

        [HttpPost]
        public async Task<ActionResult> CreateClient(Client client)
        {
            var user = _data.Users.Include("Clients").First(u => u.UserName == User.Identity.Name);
            client.Owner = user;
            ModelState.Remove("Owner");
            if (ModelState.IsValid)
            {
                user.Clients.Add(client);
                await _data.SaveChangesAsync();
                return RedirectToAction("Profile");
            }

            return View(client);
        }

        public async Task<ActionResult> DeleteClient(int id)
        {
            _data.Clients.Remove(_data.Clients.Find(id));
            await _data.SaveChangesAsync();
            return RedirectToAction("Profile");
        }
    }
}