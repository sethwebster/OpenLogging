using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SethWebster.OpenLogging.Models
{
    public class TokenLoginModel
    {
        public TokenLoginModel()
        {
            this.Kind = "client";
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Kind { get; set; }
        public Guid Apikey { get; set; }
    }
}