namespace SethWebster.OpenLogging.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removepasswordfromclient : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clients", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "Password", c => c.String(nullable: false));
        }
    }
}
