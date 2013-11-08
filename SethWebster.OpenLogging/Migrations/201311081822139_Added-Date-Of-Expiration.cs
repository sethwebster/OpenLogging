namespace SethWebster.OpenLogging.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateOfExpiration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogMessages", "DateOfExpiration", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogMessages", "DateOfExpiration");
        }
    }
}
