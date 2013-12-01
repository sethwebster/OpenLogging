using SethWebster.OpenLogging.Models.Migrations;
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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LogMessage>().HasRequired(l => l.Client).WithMany(c => c.LogMessages).WillCascadeOnDelete();
            modelBuilder.Entity<Client>().HasRequired(l => l.Owner).WithMany(c => c.Clients).WillCascadeOnDelete();
        }

    }
}