namespace BookieDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class closematch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "Close", c => c.Boolean(nullable: false));
            DropColumn("dbo.Matches", "Current");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Matches", "Current", c => c.Boolean(nullable: false));
            DropColumn("dbo.Matches", "Close");
        }
    }
}
