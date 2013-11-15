using Microsoft.AspNet.SignalR;
using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SethWebster.OpenLogging.Hubs
{
    public class LogMessageSyndication
    {
        public static async void ReportMessage(LogMessage message)
        {
            var g = GlobalHost.ConnectionManager.GetHubContext<LoggingHub>();
            //TODO: Prevent circular reference here            
            if (message.Client != null)
            {
                message.Client = null;
            }
            g.Clients.All.newLogMessage(message);
        }
    }
}