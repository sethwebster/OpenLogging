using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SethWebster.OpenLogging.Models
{

    public class Client
    {
        public Client()
        {
            this.LogMessages = new Collection<LogMessage>();
            this.DateCreated = DateTimeOffset.Now;
            this.CurrentApiKey = Guid.NewGuid();
        }
        public int ClientId { get; set; }
        [Required]
        public User Owner  { get; set; }

        [Required]
        public string ClientName { get; set; }
        public Guid CurrentApiKey { get; set; }
        public ICollection<LogMessage> LogMessages { get; set; }

        [Required]
        [JsonIgnore]
        public string Password
        {

            get;
            set;
        }
        public DateTimeOffset DateCreated { get; set; }
    }
}