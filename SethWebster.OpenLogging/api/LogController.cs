using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SethWebster.OpenLogging.api
{
    public class LogController : ApiController
    {
        DBContext data = new DBContext();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [ResponseType(typeof(LogMessage))]
        public async Task<IHttpActionResult> Post(LogMessage message)
        {
            Client cli = GetClientFromHeaders();
            cli.LogMessages.Add(message);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await data.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = message.LogMessageId }, message);
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

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}