using Newtonsoft.Json;
using SethWebster.OpenLogging.Client.Async;
using SethWebster.OpenLogging.Client.Exceptions;
using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SethWebster.OpenLogging.Client
{
    public abstract class OpenLoggingClientBase
    {
        Guid _clientApiKey = Guid.Empty;
        protected Uri _endpoint = new Uri("https://openlogger.azurewebsites.net/api");
        private AuthorizationTicket _ticket = new AuthorizationTicket();

        /// <summary>
        /// The current endpoint for API interactions
        /// </summary>
        public Uri EndPoint { get { return _endpoint; } }

        /// <summary>
        /// When overridden, provides the AuthenticationKind (client or account)
        /// </summary>
        protected abstract string AuthenticationKind { get; }
        public OpenLoggingClientBase()
        {
        }
        public OpenLoggingClientBase(Uri endpoint)
            : this()
        {
            _endpoint = endpoint;
        }
        public OpenLoggingClientBase(Guid apiKey)
            : this()
        {
            _clientApiKey = apiKey;
        }
        public OpenLoggingClientBase(Guid apiKey, Uri endpoint)
            : this()
        {
            _clientApiKey = apiKey;
            _endpoint = endpoint;
        }

        public Guid ApiKey
        {
            get { return _clientApiKey; }
            protected set { _clientApiKey = value; }
        }
        protected TReturn PerformAction<TInput, TReturn>(string actionUrl, HttpMethod method, TInput input, bool requiresAuth)
        {
            var item = PerformActionAsync<TInput, TReturn>(actionUrl, method, input, requiresAuth);
            Task.WaitAll(item);
            return item.Result;
        }
        protected async Task<TReturn> PerformActionAsync<TInput, TReturn>(string actionUrl, HttpMethod method, TInput input, bool requiresAuth)
        {
            var cli = CreateHttpClient(requiresAuth);
            HttpResponseMessage response = null;
            switch (method.ToString())
            {
                case "POST":
                    response = await cli.PostAsJsonAsync<TInput>(new Uri(actionUrl, UriKind.Relative), input);
                    break;
                case "PUT":
                    response = await cli.PutAsJsonAsync<TInput>(new Uri(actionUrl), input);
                    break;
                case "DELETE":
                    response = await cli.DeleteAsync(actionUrl + "/" + input);
                    break;
            }

            if (response.IsSuccessStatusCode)
            {
                var strRes = await response.Content.ReadAsStringAsync();

                var message2 = JsonConvert.DeserializeObject<TReturn>(strRes);
                return message2;
            }
            else
            {
                throw new HttpResponseException(response.StatusCode, response.ReasonPhrase);
            }


        }
        protected TReturn CreateItem<TInput, TReturn>(string actionUrl, TInput item, bool requiresAuth)
        {
            return PerformAction<TInput, TReturn>(actionUrl, HttpMethod.Post, item, requiresAuth);
        }
        protected async Task<TReturn> CreateItemAsync<TInput, TReturn>(string actionUrl, TInput item, bool requiresAuth)
        {
            return await PerformActionAsync<TInput, TReturn>(actionUrl, HttpMethod.Post, item, requiresAuth);
        }
        protected T DeleteItem<T>(string actionUrl, int itemId, bool requiresAuth)
        {
            return PerformAction<int, T>(actionUrl, HttpMethod.Delete, itemId, requiresAuth);
        }
        protected async Task<T> DeleteItemAsync<T>(string actionUrl, int itemId, bool requiresAuth)
        {
            return await PerformActionAsync<int, T>(actionUrl, HttpMethod.Delete, itemId, requiresAuth);
        }
        protected HttpClient CreateHttpClient(bool requiresAuth)
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
        protected async Task EnsureAuthTicket()
        {
            ValidateApiKey();
            if (_ticket.IsExpired)
            {
                try
                {
                    Trace.WriteLine("Ticket Expired, Retrieving...");
                    HttpClient cli = new HttpClient();
                    cli.BaseAddress = _endpoint;
                    _ticket = await PerformActionAsync<TokenLoginModel, AuthorizationTicket>(
                        "/api/auth/token",
                       HttpMethod.Post, new TokenLoginModel()
                       {
                           Apikey = _clientApiKey,
                           Kind = AuthenticationKind
                       }, false);

                    Trace.WriteLine(string.Format("Ticket Received: {0} {1} {2}", _ticket.Username, _ticket.AccessToken, _ticket.Expires));

                }
                catch (HttpResponseException hrex)
                {
                    Console.WriteLine(hrex);

                }
            }

        }
        protected void ValidateApiKey()
        {
            if (_clientApiKey == Guid.Empty)
            {
                throw new InvalidOperationException("API Key is not set");
            }
        }
    }

}