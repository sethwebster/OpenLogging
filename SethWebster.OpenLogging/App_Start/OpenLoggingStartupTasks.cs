using SethWebster.OpenLogging.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;

namespace SethWebster.OpenLogging.App_Start
{
    public class OpenLoggingStartupTasks
    {
        private static DBContext data = new DBContext();
        private const int TIMER_INTERVAL = 30000;
        private static Timer loopTimer;
        private static int LoopCount = 0;
        public static void Start()
        {
            loopTimer = new Timer();
            loopTimer.Interval = TIMER_INTERVAL;
            loopTimer.Elapsed += loopTimer_Elapsed;
            loopTimer.Start();
        }

        static void loopTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PurgeEligibileLogEntries();
            LoopCount++;
        }

        private static void PurgeEligibileLogEntries()
        {
            try
            {
                if (LoopCount % 2 == 0)
                {
                    var dataSet = data.LogMessages.Where(l => l.DateOfExpiration < DateTimeOffset.Now);
                    foreach (var d in dataSet)
                    {
                        data.LogMessages.Remove(d);
                    }
                    data.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }
    }
}