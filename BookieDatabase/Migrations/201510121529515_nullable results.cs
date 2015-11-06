namespace BookieDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableresults : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Matches", "HomeResult", c => c.Int());
            AlterColumn("dbo.Matches", "AwayResult", c => c.Int());
            AlterColumn("dbo.UserResults", "Home", c => c.Int());
            AlterColumn("dbo.UserResults", "Away", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserResults", "Away", c => c.Int(nullable: false));
            AlterColumn("dbo.UserResults", "Home", c => c.Int(nullable: false));
            AlterColumn("dbo.Matches", "AwayResult", c => c.Int(nullable: false));
            AlterColumn("dbo.Matches", "HomeResult", c => c.Int(nullable: false));
        }
    }
}
