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
        static DBContext _data = new DBContext();
        static Dictionary<int, string> _clientIdLookup = new Dictionary<int, string>();

        public static async void ReportMessage(LogMessage message)
        {
            var g = GlobalHost.ConnectionManager.GetHubContext<LoggingHub>();
            var clientId = message.Client.ClientId;
            var userId = LookupUserId(message);
            message = message.Clone();
            //TODO: Prevent circular reference here 
            message.Client.LogMessages = null;
            g.Clients.User(userId).newLogMessage(message);
        }


        private static string LookupUserId(LogMessage message)
        {
            if (!_clientIdLookup.ContainsKey(message.Client.ClientId))
            {
                _clientIdLookup.Add(message.Client.ClientId, _data.Clients.Include("Owner").First(c => c.ClientId == message.Client.ClientId).Owner.UserName);
            }
            return _clientIdLookup[message.Client.ClientId];
        }
    }
}