using SethWebster.OpenLogging.Client;
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
            // uri = "http://openlogger/api/";
            // Uncomment this line when not using Fiddler
            // uri = "http://localhost:6=757/api/";
            OpenLoggingClient cl = new Client.OpenLoggingClient(new Uri(uri));
            var clientCreationResult = await CreateClient(uri, cl);
            var logCreationResult = await CreateLogEntry(uri, clientCreationResult);
            var deleteClientResult = await DeleteClient(clientCreationResult, cl);
            return logCreationResult;
            //Writeline("Press ENTER to start.");
            //System.Console.ReadLine();
            //System.Console.WriteLine("Server is located at " + uri);
            //OpenLogging.Client.OpenLoggingClient cl = new Client.OpenLoggingClient(new Uri(uri));
            //cl.CreateClient(new SethWebster.OpenLogging.Models.Client()
            //{
            //    ClientName = "southwest"
            //}).ContinueWith(clientResult =>
            //{
            //    if (!clientResult.IsFaulted)
            //    {
            //        System.Console.WriteLine("Client created.");
            //        Writeline("Press ENTER to create log entry"); System.Console.ReadLine();
            //        var newcl = new Client.OpenLoggingClient(clientResult.Result.CurrentApiKey, new Uri(uri));
            //        System.Console.WriteLine("Creating new log entry");
            //        newcl.NewLogEntry(new SethWebster.OpenLogging.Models.LogMessage
            //        {
            //            Category = "Error",
            //            Message = "Message 2",
            //            Title = "Big error"
            //        }).ContinueWith(logEntryResult =>
            //        {
            //            if (!logEntryResult.IsFaulted)
            //            {
            //                System.Console.WriteLine("Created");
            //                System.Console.WriteLine(logEntryResult.Result.LogMessageId);
            //                Writeline("Press ENTER to DELETE client"); System.Console.ReadLine();
            //                System.Console.WriteLine("Deleting client");
            //                newcl.DeleteClient(clientResult.Result).ContinueWith(deleteClientResult =>
            //                {
            //                    if (!deleteClientResult.IsFaulted)
            //                    {
            //                        System.Console.WriteLine("Client deleted " + deleteClientResult);
            //                    }
            //                    else
            //                    {
            //                        Writeline("Error: " + clientResult.Exception);
            //                    }
            //                });
            //            }
            //            else
            //            {
            //                Writeline("Error: " + logEntryResult.Exception);
            //            }
            //        });
            //    }
            //    else
            //    {
            //        Writeline("Error: " + clientResult.Exception);
            //    }
            //});


            //System.Console.ReadLine();

        }

        private static async Task<Models.Client> DeleteClient(Models.Client client, OpenLoggingClient cl)
        {
            Writeline("Deleting Client #" + client.ClientId);
            var res = await cl.DeleteClient(client);
            return res;
        }

        private static async Task<LogMessage> CreateLogEntry(string uri, Models.Client clientCreationResult)
        {
            Writeline("Creating Log Message Entry ...");
            var cl2 = new OpenLoggingClient(clientCreationResult.CurrentApiKey, new Uri(uri));

            var logRes = await cl2.NewLogEntry(new LogMessage()
            {
                Title = "New Log MEssage" + DateTime.Now,
                Category = "ERROR",
                Body = "Body of " + DateTime.Now,
                Message = "This is the dang message"
            });
            return logRes;
        }

        private static async Task<Models.Client> CreateClient(string uri, OpenLoggingClient cl)
        {
            Writeline("Using " + uri + " as endpoint for operations");
            Writeline("Creating Client...");
            var clientCreationResult = await cl.CreateClient(new SethWebster.OpenLogging.Models.Client() { ClientName = "Southwest" });
            Writeline("Created Client #" + clientCreationResult.ClientId);
            return clientCreationResult;
        }
        static void Main(string[] args)
        {
            var item = dostuff();

            try
            {
                Task.WaitAll(item);
            }
            catch(Exception e)
            {
                Writeline("ERROR: " + e.GetBaseException());
            }
            Writeline(item.Result.LogMessageId);
            Readline();
        }

        static string Readline() { return System.Console.ReadLine(); }

        static void Writeline(object content)
        {
            System.Console.WriteLine(content);
        }
    }
}
