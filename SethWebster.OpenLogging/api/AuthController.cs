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
        public async Task<IHttpActionResult> Token(TokenLoginModel model)
        {
            var user = _data.Clients.FirstOrDefault(c => c.ClientName == model.UserName && c.Password == model.Password);
            if (user == null)
            {
                return this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }

            var identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));
            AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
            return this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<object>(new
                {
                    UserName = model.UserName,
                    AccessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket)
                }, Configuration.Formatters.JsonFormatter)
            });

        }
    }
}
