using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Title { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Message { get; set; }
        public string Body { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateOfEvent { get; set; }
        [Required]
        public Client Client { get; set; }
    }
}