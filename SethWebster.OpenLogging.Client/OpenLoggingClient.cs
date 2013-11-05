using Newtonsoft.Json;
using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SethWebster.OpenLogging.Client
{
    public class OpenLoggingClient
    {
        Guid _clientApiKey = Guid.Empty;
        Uri _endpoint = new Uri("https://openlogging.azurewebsites.net/api");
        public OpenLoggingClient()
        {

        }
        public OpenLoggingClient(Guid apiKey)
        {
            _clientApiKey = apiKey;
        }

        public OpenLoggingClient(Guid apiKey, Uri endpoint)
        {
            _clientApiKey = apiKey;
            _endpoint = endpoint;
        }
        public async Task<LogMessage> NewLogEntry(LogMessage message)
        {
            ValidateApiKey();
            HttpClient cli = new HttpClient();
            cli.BaseAddress = _endpoint;
            cli.DefaultRequestHeaders.Add("x-auth", _clientApiKey.ToString());
            var res = await cli.PostAsJsonAsync<LogMessage>(new Uri("/api/log",UriKind.Relative), message);
            var strRes = await res.Content.ReadAsStringAsync();
            var message2 = JsonConvert.DeserializeObject<LogMessage>(strRes);

            return message2;
        }

        private void ValidateApiKey()
        {
            if (_clientApiKey == Guid.Empty)
            {
                throw new InvalidOperationException("API Key is not set");
            }
        }
    }
}
