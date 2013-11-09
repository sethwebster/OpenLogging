using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SethWebster.OpenLogging.Client
{
    public class AuthorizationTicket
    {
        public string Username { get; set; }
        public string TicketData { get; set; }
        public DateTimeOffset Expires { get; set; }
        public bool IsExpired
        {
            get
            {
                return DateTimeOffset.Now > Expires;
            }
        }

        public TimeSpan TimeUntilExpires
        {
            get
            {
                return Expires - DateTimeOffset.Now;
            }
        }
    }
}
