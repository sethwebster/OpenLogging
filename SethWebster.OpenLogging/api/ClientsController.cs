using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SethWebster.OpenLogging.Models;

namespace SethWebster.OpenLogging.api
{
    public class ClientsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET api/Clients
        public IQueryable<Client> GetClients()
        {
            return db.Clients;
        }

        // GET api/Clients/5
        [ResponseType(typeof(Client))]
        [Authorize]
        public async Task<IHttpActionResult> GetClient(int id)
        {
            Client client = await db.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [ResponseType(typeof(Client))]
        [Authorize]
        public async Task<IHttpActionResult> GetClient(string name)
        {
            Client client = await db.Clients.FirstOrDefaultAsync(c => c.ClientName.Equals(name));
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // PUT api/Clients/5
        [Authorize]
        public async Task<IHttpActionResult> PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Clients
        [ResponseType(typeof(Client))]
        [Authorize]
        public async Task<IHttpActionResult> PostClient(ClientCreationModel clientModel)
        {
            var owner = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (owner==null)
            {
                return this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Client client = new Client()
            {
                ClientName = clientModel.ClientName,
                Owner = owner
            };

            db.Clients.Add(client);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = client.ClientId }, client);
        }

        // DELETE api/Clients/5
        [ResponseType(typeof(Client))]
        public async Task<IHttpActionResult> DeleteClient(int id)
        {
            Client client = await db.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Clients.Remove(client);
            await db.SaveChangesAsync();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Clients.Count(e => e.ClientId == id) > 0;
        }
    }
}