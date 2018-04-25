namespace Rowa.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProfilePiccolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInformation", "ProfilePic", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInformation", "ProfilePic");
        }
    }
}
