using SethWebster.OpenLogging.Models;
using System;
using System.Threading.Tasks;

namespace SethWebster.OpenLogging.Client
{
    
    /// <summary>
    /// For creating log entries
    /// </summary>
    public class OpenLoggingClient : OpenLoggingClientBase
    {

        public OpenLoggingClient()
            : base()
        {
        }
        public OpenLoggingClient(Uri endpoint)
            : base(endpoint) { }
        public OpenLoggingClient(Guid apiKey)
            : base(apiKey)
        {

        }
        public OpenLoggingClient(Guid apiKey, Uri endpoint)
            : base(apiKey, endpoint)
        {

        }
        public async Task<LogMessage> NewLogEntry(LogMessage message)
        {
            var res = await CreateItem<LogMessage, LogMessage>("/api/log", message, true);
            return res;
        }

        protected override string AuthenticationKind
        {
            get { return "client"; }
        }

    }
}
