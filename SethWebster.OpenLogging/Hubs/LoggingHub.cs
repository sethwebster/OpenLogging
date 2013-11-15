using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SethWebster.OpenLogging.Models;

namespace SethWebster.OpenLogging.Hubs
{
    public class LoggingHub : Hub
    {
        public void NewLogMessage(LogMessage message)
        {
            Clients.All.newLogMessage(message);
        }
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}