namespace BookieDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class result : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "HomeResult", c => c.Int(nullable: false));
            AddColumn("dbo.Matches", "AwayResult", c => c.Int(nullable: false));
            AddColumn("dbo.UserResults", "ComfirmResult", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserResults", "ComfirmResult");
            DropColumn("dbo.Matches", "AwayResult");
            DropColumn("dbo.Matches", "HomeResult");
        }
    }
}
