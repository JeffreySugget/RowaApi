namespace Rowa.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Createsecrettable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Secret",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Jwt = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Secret");
        }
    }
}
