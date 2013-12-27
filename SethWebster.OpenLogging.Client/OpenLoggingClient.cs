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
        const string ENDPOINT_LOG = "/api/log";
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
        public LogMessage NewLogEntry(LogMessage message)
        {
            var res = CreateItem<LogMessage, LogMessage>(ENDPOINT_LOG, message, true);
            return res;
        }
        public async Task<LogMessage> NewLogEntryAsync(LogMessage message)
        {
            var res = await CreateItemAsync<LogMessage, LogMessage>(ENDPOINT_LOG, message, true);
            return res;
        }

        protected override string AuthenticationKind
        {
            get { return "client"; }
        }

    }
}
