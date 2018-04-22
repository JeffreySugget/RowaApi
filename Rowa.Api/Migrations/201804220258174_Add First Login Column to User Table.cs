namespace Rowa.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFirstLoginColumntoUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "FirstLogin", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "FirstLogin");
        }
    }
}
