using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SethWebster.OpenLogging.Client
{
    public class OpenLoggingAccountClient : OpenLoggingClientBase
    {
        public OpenLoggingAccountClient()
            : base()
        {
        }
        public OpenLoggingAccountClient(Uri endpoint)
            : base(endpoint) { }
        public OpenLoggingAccountClient(Guid apiKey)
            : base(apiKey)
        {

        }
        public OpenLoggingAccountClient(Guid apiKey, Uri endpoint)
            : base(apiKey, endpoint)
        {

        }

        public async Task<SethWebster.OpenLogging.Models.Client> CreateClient(SethWebster.OpenLogging.Models.Client client)
        {
            var res = await CreateItem<object, SethWebster.OpenLogging.Models.Client>("/api/clients", new { client.ClientName }, true);
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
            if (ApiKey != Guid.Empty)
            {
                cli.DefaultRequestHeaders.Add("x-auth", ApiKey.ToString());
            }
            var res = await cli.GetAsync("/api/clients");
            var strRes = await res.Content.ReadAsStringAsync();
            var message2 = JsonConvert.DeserializeObject<IEnumerable<Models.Client>>(strRes);
            return message2;
        }


        protected override string AuthenticationKind
        {
            get { return "account"; }
        }
    }
}
