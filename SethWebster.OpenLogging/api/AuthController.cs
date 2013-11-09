using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SethWebster.OpenLogging.api
{
    public class AuthController : ApiController
    {
        DBContext _data = new DBContext();
        public async Task<IHttpActionResult> Token(TokenLoginModel model)
        {
            
        }
    }
}
