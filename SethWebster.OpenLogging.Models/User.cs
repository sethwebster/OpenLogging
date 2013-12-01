using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SethWebster.OpenLogging.Models
{
    [Table("AspNetUsers")]
    public class User
    {
        public User()
        {
            this.Clients = new Collection<Client>();
            this.UserApiKey = Guid.NewGuid();
        }
        [MaxLength(128)]
        [Key]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Discriminator { get; set; }
        public Guid UserApiKey { get; set; }
        public ICollection<Client> Clients { get; set; }
    }
}