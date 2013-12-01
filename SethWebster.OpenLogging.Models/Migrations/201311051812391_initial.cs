namespace SethWebster.OpenLogging.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        CurrentApiKey = c.Guid(nullable: false),
                        DateCreated = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.LogMessages",
                c => new
                    {
                        LogMessageId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Category = c.String(),
                        Message = c.String(),
                        Body = c.String(),
                        DateCreated = c.DateTimeOffset(nullable: false, precision: 7),
                        DateOfEvent = c.DateTimeOffset(nullable: false, precision: 7),
                        Client_ClientId = c.Int(),
                    })
                .PrimaryKey(t => t.LogMessageId)
                .ForeignKey("dbo.Clients", t => t.Client_ClientId)
                .Index(t => t.Client_ClientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogMessages", "Client_ClientId", "dbo.Clients");
            DropIndex("dbo.LogMessages", new[] { "Client_ClientId" });
            DropTable("dbo.LogMessages");
            DropTable("dbo.Clients");
        }
    }
}
