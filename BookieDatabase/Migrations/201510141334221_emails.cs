namespace BookieDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "EmailCanceled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "EmailCanceled");
        }
    }
}
