namespace Rowa.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedprofilepicpathcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInformation", "ProfilePicPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInformation", "ProfilePicPath");
        }
    }
}
