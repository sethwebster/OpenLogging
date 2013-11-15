using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SethWebster.OpenLogging.api
{
    public class AuthController : ApiController
    {
        DBContext _data = new DBContext();
        [HttpPost]
        public IHttpActionResult Token(TokenLoginModel model)
        {
            if (model==null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                Client user = TryFetchUser(model);
                
                if (user == null)
                {
                    return this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }

                if (model.Apikey!= default(Guid) && user.CurrentApiKey!=model.Apikey)
                {
                    return BadRequest("Api Key Invalid");
                }

                return this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = GetTicket(user)
                });
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        private Client TryFetchUser(TokenLoginModel model)
        {
            Client user = null;
            if (model.Apikey != (default(Guid)) && user == null)
            {
                user = _data.Clients.FirstOrDefault(c => c.CurrentApiKey == model.Apikey);
            }
            return user;
        }

        public ObjectContent<object> GetTicket(Client client)
        {
            var identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, client.ClientName));
            AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
            return new ObjectContent<object>(new
            {
                UserName = client.ClientName,
                AccessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket),
                Expires = ticket.Properties.ExpiresUtc
            }, Configuration.Formatters.JsonFormatter);
        }
    }
}
