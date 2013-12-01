namespace SethWebster.OpenLogging.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedrequiredfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "ClientName", c => c.String(nullable: false));
            AlterColumn("dbo.LogMessages", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.LogMessages", "Category", c => c.String(nullable: false));
            AlterColumn("dbo.LogMessages", "Message", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LogMessages", "Message", c => c.String());
            AlterColumn("dbo.LogMessages", "Category", c => c.String());
            AlterColumn("dbo.LogMessages", "Title", c => c.String());
            AlterColumn("dbo.Clients", "ClientName", c => c.String());
        }
    }
}
