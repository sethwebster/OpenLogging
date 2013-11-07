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
    public class LogController : ApiController
    {
        private DBContext data = new DBContext();

        // GET api/Log
        public IQueryable<LogMessage> GetLogMessages()
        {
            return data.LogMessages;
        }

        // GET api/Log/5
        [ResponseType(typeof(LogMessage))]
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
        public async Task<IHttpActionResult> PostLogMessage(LogMessage logmessage)
        {
            var client = GetClientFromHeaders();
            client.LogMessages.Add(logmessage);
            logmessage.Client = client;
            ModelState.Remove("logmessage.Client");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await data.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = logmessage.LogMessageId }, logmessage);
        }

        // DELETE api/Log/5
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
            KeyValuePair<string, IEnumerable<string>> header;
            Guid clientValue = Guid.Empty;
            try
            {
                header = Request.Headers.First(h => h.Key.Equals("x-auth"));

            }
            catch (InvalidOperationException iox)
            {
                throw new InvalidOperationException("Authorization Header Not Found", iox);
            }
            try
            {
                clientValue = Guid.Parse(header.Value.First());
            }
            catch
            {
                throw new InvalidOperationException("API Key is not properly formatted");
            }

            try
            {
                return data.Clients.First(c => c.CurrentApiKey.Equals(clientValue));
            }
            catch (InvalidOperationException iox)
            {
                throw new InvalidOperationException("Client Header Value is not valid", iox);
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