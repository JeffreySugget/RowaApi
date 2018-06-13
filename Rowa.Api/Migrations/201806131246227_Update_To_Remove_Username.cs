namespace Rowa.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_To_Remove_Username : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Email", c => c.String());
            DropColumn("dbo.UserInformation", "Email");
            DropColumn("dbo.User", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Username", c => c.String());
            AddColumn("dbo.UserInformation", "Email", c => c.String());
            DropColumn("dbo.User", "Email");
        }
    }
}
