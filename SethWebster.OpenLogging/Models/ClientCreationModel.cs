using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SethWebster.OpenLogging.Models
{
    public class ClientCreationModel
    {
        public ClientCreationModel()
        {
            this.LogMessages = new Collection<LogMessage>();
            this.DateCreated = DateTimeOffset.Now;
            this.CurrentApiKey = Guid.NewGuid();
        }
        public int ClientId { get; set; }
        [Required]
        public string ClientName { get; set; }
        public Guid CurrentApiKey { get; set; }
        public ICollection<LogMessage> LogMessages { get; set; }
        [Required]

        
        public string Password
        {

            get;
            set;
        }
        public DateTimeOffset DateCreated { get; set; }
    }
}