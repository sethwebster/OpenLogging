namespace SethWebster.OpenLogging.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedclientstouser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Owner_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Clients", "Owner_Id");
            AddForeignKey("dbo.Clients", "Owner_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Clients", new[] { "Owner_Id" });
            DropColumn("dbo.Clients", "Owner_Id");
           
        }
    }
}
 