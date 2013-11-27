namespace SethWebster.OpenLogging.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcontexttologmessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogMessages", "Context", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogMessages", "Context");
        }
    }
}
