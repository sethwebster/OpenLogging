namespace SethWebster.OpenLogging.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedapikeyataccountlevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserApiKey", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserApiKey");
        }
    }
}
