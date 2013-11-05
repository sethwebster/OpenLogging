using SethWebster.OpenLogging.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SethWebster.OpenLogging.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DBContext"){
            Database.SetInitializer<DBContext>(new MigrateDatabaseToLatestVersion<DBContext, Configuration>());
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<LogMessage> LogMessages { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LogMessage>().HasRequired(l => l.Client).WithMany(c => c.LogMessages).WillCascadeOnDelete();
        }

    }
}