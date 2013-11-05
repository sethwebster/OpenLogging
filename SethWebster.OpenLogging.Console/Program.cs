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
            OpenLogging.Client.OpenLoggingClient cl = new Client.OpenLoggingClient(new Guid("0a21dac6-f311-4887-96e7-fa59441efe21"), new Uri("http://localhost:60757/api"));

            var res = cl.NewLogEntry(new SethWebster.OpenLogging.Models.LogMessage
            {
                Category = "Error",
                Message = "Message 2",
                Title = "Big error"
            }).ContinueWith(r =>
            {
                System.Console.WriteLine(r.Result.LogMessageId);
            });
            System.Console.ReadLine();
        }
    }
}
