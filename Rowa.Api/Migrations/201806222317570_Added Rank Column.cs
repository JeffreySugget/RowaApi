namespace Rowa.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRankColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInformation", "Rank", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInformation", "Rank");
        }
    }
}
