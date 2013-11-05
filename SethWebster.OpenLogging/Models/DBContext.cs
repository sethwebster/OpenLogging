using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SethWebster.OpenLogging.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DBContext"){}

        public DbSet<Client> Clients { get; set; }
        public DbSet<LogMessage> LogMessages { get; set; }
    }
}