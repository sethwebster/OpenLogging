namespace SethWebster.OpenLogging.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedrequiredtoclient : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Clients", new[] { "Owner_Id" });
            AlterColumn("dbo.Clients", "Owner_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Clients", "Owner_Id");
            AddForeignKey("dbo.Clients", "Owner_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Clients", new[] { "Owner_Id" });
            AlterColumn("dbo.Clients", "Owner_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Clients", "Owner_Id");
            AddForeignKey("dbo.Clients", "Owner_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
