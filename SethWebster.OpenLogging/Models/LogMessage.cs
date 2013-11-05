using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SethWebster.OpenLogging.Models
{
    public class LogMessage
    {
        public LogMessage()
        {
            this.DateCreated = DateTimeOffset.Now;
            this.DateOfEvent = DateTimeOffset.Now;
        }
        public int LogMessageId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public string Body { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateOfEvent { get; set; }
    }
}