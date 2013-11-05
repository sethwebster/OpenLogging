using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SethWebster.OpenLogging.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string uri = "https://openlogging.azurewebsites.net/api";
                //uri = "http://localhost:60757/api/";
                System.Console.WriteLine("Creating client to " + uri);
                OpenLogging.Client.OpenLoggingClient cl = new Client.OpenLoggingClient(new Uri(uri));
                cl.CreateClient(new SethWebster.OpenLogging.Models.Client()
                {
                    ClientName = "southwest"
                }).ContinueWith(r =>
                {
                    System.Console.WriteLine("Client created.");
                    var newcl = new Client.OpenLoggingClient(r.Result.CurrentApiKey, new Uri(uri));
                    System.Console.WriteLine("Creating new log entry");
                    newcl.NewLogEntry(new SethWebster.OpenLogging.Models.LogMessage
                    {
                        Category = "Error",
                        Message = "Message 2",
                        Title = "Big error"
                    }).ContinueWith(rr =>
                    {
                        System.Console.WriteLine("Created");
                        System.Console.WriteLine(rr.Result.LogMessageId);
                        System.Console.WriteLine("Deleteing client");
                        newcl.DeleteClient(r.Result).ContinueWith(rrr => {
                            System.Console.WriteLine("Client deleted "+rrr);
                        });
                    });
                });


                System.Console.ReadLine();
            }
        }
    }
}
