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
using SethWebster.OpenLogging.Hubs;

namespace SethWebster.OpenLogging.api
{
    public class LogController : ApiController
    {
        private DBContext data = new DBContext();

        [Authorize]
        // GET api/Log
        public IQueryable<LogMessage> GetLogMessages()
        {
            return data.LogMessages;
        }

        // GET api/Log/5
        [ResponseType(typeof(LogMessage))]
        [Authorize]
        public async Task<IHttpActionResult> GetLogMessage(int id)
        {
            LogMessage logmessage = await data.LogMessages.FindAsync(id);
            if (logmessage == null)
            {
                return NotFound();
            }

            return Ok(logmessage);
        }

        // PUT api/Log/5
        [Authorize]
        public async Task<IHttpActionResult> PutLogMessage(int id, LogMessage logmessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logmessage.LogMessageId)
            {
                return BadRequest();
            }

            data.Entry(logmessage).State = EntityState.Modified;

            try
            {
                await data.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogMessageExists(id))
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

        // POST api/Log
        [ResponseType(typeof(LogMessage))]
        [Authorize]
        public async Task<IHttpActionResult> PostLogMessage(LogMessage logmessage)
        {
            var client = GetClientFromHeaders();
            client.LogMessages.Add(logmessage);
            if (null==logmessage)
            {
                return BadRequest("No log message was present");
            }
            logmessage.Client = client;
            ModelState.Remove("logmessage.Client");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await data.SaveChangesAsync();
            LogMessageSyndication.ReportMessage(logmessage);
            return CreatedAtRoute("DefaultApi", new { id = logmessage.LogMessageId }, logmessage);
        }

        // DELETE api/Log/5
        [Authorize]
        [ResponseType(typeof(LogMessage))]
        public async Task<IHttpActionResult> DeleteLogMessage(int id)
        {
            LogMessage logmessage = await data.LogMessages.FindAsync(id);
            if (logmessage == null)
            {
                return NotFound();
            }

            data.LogMessages.Remove(logmessage);
            await data.SaveChangesAsync();

            return Ok(logmessage);
        }
        private Client GetClientFromHeaders()
        {
            try
            {
                return data.Clients.First(c => c.ClientName == User.Identity.Name);
            }
            catch (InvalidOperationException iox)
            {
                throw new InvalidOperationException("Authentication Ticket Not Valid", iox);
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                data.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogMessageExists(int id)
        {
            return data.LogMessages.Count(e => e.LogMessageId == id) > 0;
        }
    }
}