using Newtonsoft.Json;
using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SethWebster.OpenLogging.Client
{
    public class OpenLoggingClient
    {
        static int numb = 0;
        Guid _clientApiKey = Guid.Empty;
        Uri _endpoint = new Uri("https://openlogger.azurewebsites.net/api");
        private AuthorizationTicket _ticket = new AuthorizationTicket();

        #region CTOR
        public OpenLoggingClient()
        {
            numb++;
            Console.WriteLine("Created OpenLoggingClient #" + numb);
        }
        public OpenLoggingClient(Uri endpoint)
            : this()
        {
            _endpoint = endpoint;
        }
        public OpenLoggingClient(Guid apiKey)
            : this()
        {
            _clientApiKey = apiKey;
        }
        public OpenLoggingClient(Guid apiKey, Uri endpoint)
            : this()
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
            var res = await CreateItem<LogMessage, LogMessage>("/api/log", message, true);
            return res;
        }
        public async Task<SethWebster.OpenLogging.Models.Client> CreateClient(SethWebster.OpenLogging.Models.Client client)
        {
            var res = await CreateItem<object, SethWebster.OpenLogging.Models.Client>("/api/clients", new { client.ClientName, client.Password }, false);
            return (SethWebster.OpenLogging.Models.Client)res;
        }
        public async Task<SethWebster.OpenLogging.Models.Client> DeleteClient(SethWebster.OpenLogging.Models.Client client)
        {
            var res = await DeleteItem<SethWebster.OpenLogging.Models.Client>("/api/clients", client.ClientId, true);
            return res;
        }
        public async Task<SethWebster.OpenLogging.Models.Client> GetClient(string name)
        {
            var cli = CreateHttpClient(true);
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

        private async Task<TReturn> CreateItem<TInput, TReturn>(string actionUrl, TInput item, bool requiresAuth)
        {
            var cli = CreateHttpClient(requiresAuth);
            var res = await cli.PostAsJsonAsync<TInput>(new Uri(actionUrl, UriKind.Relative), item);
            var strRes = await res.Content.ReadAsStringAsync();
            var message2 = JsonConvert.DeserializeObject<TReturn>(strRes);
            return message2;
        }

        private async Task<T> DeleteItem<T>(string actionUrl, int itemId, bool requiresAuth)
        {
            var cli = CreateHttpClient(requiresAuth);
            var res = await cli.DeleteAsync(actionUrl + "/" + itemId);
            var strRes = await res.Content.ReadAsStringAsync();
            var message2 = JsonConvert.DeserializeObject<T>(strRes);
            return message2;
        }

        private HttpClient CreateHttpClient(bool requiresAuth)
        {
            var cli = new HttpClient();
            cli.BaseAddress = _endpoint;
            if (requiresAuth)
            {
                Task.WaitAll(EnsureAuthTicket());
                cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _ticket.AccessToken);
            }
            return cli;
        }

        private async Task EnsureAuthTicket()
        {
            ValidateApiKey();
            if (_ticket.IsExpired)
            {
                Console.WriteLine("Ticket Expired, Retrieving...");
                HttpClient cli = new HttpClient();
                cli.BaseAddress = _endpoint;
                var res = await cli.PostAsJsonAsync(new Uri("/api/auth/token", UriKind.Relative), new TokenLoginModel()
                {
                    Apikey = _clientApiKey
                });
                _ticket = await res.Content.ReadAsAsync<AuthorizationTicket>();
                Console.WriteLine("Ticket Received: {0} {1} {2}", _ticket.Username, _ticket.AccessToken, _ticket.Expires);
            }

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
