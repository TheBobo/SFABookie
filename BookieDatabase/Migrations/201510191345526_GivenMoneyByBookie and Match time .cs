namespace BookieDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GivenMoneyByBookieandMatchtime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "TimeToBet", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserResults", "GivenMoneyByBookie", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserResults", "GivenMoneyByBookie");
            DropColumn("dbo.Matches", "TimeToBet");
        }
    }
}
