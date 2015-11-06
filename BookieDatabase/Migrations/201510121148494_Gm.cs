namespace BookieDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserResults", "GivenMoney", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserResults", "GivenMoney");
        }
    }
}
