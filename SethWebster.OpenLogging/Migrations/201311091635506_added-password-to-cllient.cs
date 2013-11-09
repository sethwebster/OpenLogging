namespace SethWebster.OpenLogging.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpasswordtocllient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "Password");
        }
    }
}
