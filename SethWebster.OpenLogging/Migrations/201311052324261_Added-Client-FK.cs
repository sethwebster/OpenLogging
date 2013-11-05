namespace SethWebster.OpenLogging.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClientFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LogMessages", "Client_ClientId", "dbo.Clients");
            DropIndex("dbo.LogMessages", new[] { "Client_ClientId" });
            AlterColumn("dbo.LogMessages", "Client_ClientId", c => c.Int(nullable: false));
            CreateIndex("dbo.LogMessages", "Client_ClientId");
            AddForeignKey("dbo.LogMessages", "Client_ClientId", "dbo.Clients", "ClientId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogMessages", "Client_ClientId", "dbo.Clients");
            DropIndex("dbo.LogMessages", new[] { "Client_ClientId" });
            AlterColumn("dbo.LogMessages", "Client_ClientId", c => c.Int());
            CreateIndex("dbo.LogMessages", "Client_ClientId");
            AddForeignKey("dbo.LogMessages", "Client_ClientId", "dbo.Clients", "ClientId");
        }
    }
}
