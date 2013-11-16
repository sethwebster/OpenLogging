﻿using SethWebster.OpenLogging.Client;
using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SethWebster.OpenLogging.Console
{
    class Program
    {
        static async Task<LogMessage> dostuff()
        {
            // Production URL
            string uri = "http://openlogger.azurewebsites.net/api";
            // "openlogger" hostname used below is so that we can debug with Fidder; this requires
            // updates to the CustomRules of Fiddler.
            // See http://codebetter.com/howarddierking/2011/05/09/getting-fiddler-and-the-net-framework-to-play-better-together-2/ for usage
            // Uncomment this line when using Fiddler (and set up as above)
            uri = "http://openlogger/api/";
            // Uncomment this line when not using Fiddler
            // uri = "http://localhost:60757/api/";
            Guid accountApiKey = new Guid("BF59BE01-19C4-4517-BF76-6A0AE4F24720");
            OpenLoggingAccountClient acctClient = new OpenLoggingAccountClient(accountApiKey, new Uri(uri));

            Writeline("Using " + uri + " as endpoint for operations");
            var clientCreationResult = await CreateClient(uri, acctClient);
            var clientGetResult = await acctClient.GetClient("Southwest");
            OpenLoggingClient cl = new Client.OpenLoggingClient(clientCreationResult.CurrentApiKey, new Uri(uri));
            Writeline(clientGetResult.ClientName + " " + clientGetResult.CurrentApiKey + " fetched OK");
            var logCreationResult = await CreateLogEntry(uri, cl);
            var listClientsResult = await ListClients(acctClient);
            return logCreationResult;


        }

        private static async Task<object> ListClients(OpenLoggingAccountClient cl)
        {
            var res = await cl.ListClients();
            Writeline(res.Count() + " clients");
            foreach (var c in res)
            {
                Writeline(c.ClientId + " " + c.ClientName + " " + c.CurrentApiKey);

            }
            return res;
        }

        private static async Task<Models.Client> DeleteClient(Models.Client client, OpenLoggingAccountClient cl)
        {
            Writeline("Deleting Client #" + client.ClientId);
            var res = await cl.DeleteClient(client);
            return res;
        }

        private static async Task<LogMessage> CreateLogEntry(string uri, OpenLoggingClient cl2)
        {
            Writeline("Creating Log Message Entry ...");

            var logRes = await cl2.NewLogEntry(new LogMessage()
            {
                Title = "New Log MEssage" + DateTime.Now,
                Category = "ERROR",
                Body = "Body of " + DateTime.Now,
                Message = "This is the dang message",
                DateOfExpiration = DateTimeOffset.Now.AddMinutes(1)
            });
            return logRes;
        }

        private static async Task<Models.Client> CreateClient(string uri, OpenLoggingAccountClient cl)
        {
            Writeline("Creating Client...");
            var clientCreationResult = await cl.CreateClient(new SethWebster.OpenLogging.Models.Client() { ClientName = "Southwest" });
            Writeline("Created Client #" + clientCreationResult.ClientId);
            return clientCreationResult;
        }
        static void Main(string[] args)
        {
            while (true)
            {
                var item = dostuff();

                try
                {
                    Task.WaitAll(item);
                }
                catch (Exception e)
                {
                    Writeline("ERROR: " + e.GetBaseException());
                }
                Writeline(item.Result.LogMessageId);
                Writeline("Press ENTER to repeat");
                Readline();
            }
        }

        static string Readline() { return System.Console.ReadLine(); }

        static void Writeline(object content)
        {
            System.Console.WriteLine(content);
        }
    }
}
