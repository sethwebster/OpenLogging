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
        Uri _endpoint = new Uri("https://openlogger.azurewebsites.net/api");
        private AuthorizationTicket _ticket;

        #region CTOR
        public OpenLoggingClient()
        {

        }
        public OpenLoggingClient(Uri endpoint)
        {
            _endpoint = endpoint;
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
        #endregion

        public Guid ApiKey
        {
            get { return _clientApiKey; }
            protected set { _clientApiKey = value; }
        }
        public async Task<LogMessage> NewLogEntry(LogMessage message)
        {
            EnsureAuthTicket();
            var res = await CreateItem<LogMessage>("/api/log", message);
            return res;
        }
        public async Task<SethWebster.OpenLogging.Models.Client> CreateClient(SethWebster.OpenLogging.Models.Client client)
        {
            var res = await CreateItem<SethWebster.OpenLogging.Models.Client>("/api/clients", client);
            return res;
        }
        public async Task<SethWebster.OpenLogging.Models.Client> DeleteClient(SethWebster.OpenLogging.Models.Client client)
        {
            var res = await DeleteItem<SethWebster.OpenLogging.Models.Client>("/api/clients", client.ClientId);
            return res;
        }
        public async Task<SethWebster.OpenLogging.Models.Client> GetClient(string name)
        {
            HttpClient cli = new HttpClient();
            cli.BaseAddress = _endpoint;

            var res = await cli.GetAsync("/api/clients?name=" + name);
            var strRes = await res.Content.ReadAsStringAsync();
            var message2 = JsonConvert.DeserializeObject<Models.Client>(strRes);
            return message2;
        }
        public async Task<IEnumerable<Models.Client>> ListClients()
        {
            HttpClient cli = new HttpClient();
            cli.BaseAddress = _endpoint;
            if (_clientApiKey != Guid.Empty)
            {
                cli.DefaultRequestHeaders.Add("x-auth", _clientApiKey.ToString());
            }
            var res = await cli.GetAsync("/api/clients");
            var strRes = await res.Content.ReadAsStringAsync();
            var message2 = JsonConvert.DeserializeObject<IEnumerable<Models.Client>>(strRes);
            return message2;
        }

        private async Task<T> CreateItem<T>(string actionUrl, T item)
        {
            HttpClient cli = new HttpClient();
            cli.BaseAddress = _endpoint;
            if (_clientApiKey != Guid.Empty)
            {
                cli.DefaultRequestHeaders.Add("x-auth", _clientApiKey.ToString());
            }
            var res = await cli.PostAsJsonAsync<T>(new Uri(actionUrl, UriKind.Relative), item);
            var strRes = await res.Content.ReadAsStringAsync();
            var message2 = JsonConvert.DeserializeObject<T>(strRes);
            return message2;
        }

        private async Task<T> DeleteItem<T>(string actionUrl, int itemId)
        {
            HttpClient cli = new HttpClient();
            cli.BaseAddress = _endpoint;
            if (_clientApiKey != Guid.Empty)
            {
                cli.DefaultRequestHeaders.Add("x-auth", _clientApiKey.ToString());
            }
            var res = await cli.DeleteAsync(actionUrl + "/" + itemId);
            var strRes = await res.Content.ReadAsStringAsync();
            var message2 = JsonConvert.DeserializeObject<T>(strRes);
            return message2;
        }
        private void EnsureAuthTicket()
        {
            ValidateApiKey();

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
