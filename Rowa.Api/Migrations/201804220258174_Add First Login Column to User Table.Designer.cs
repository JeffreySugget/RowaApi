// <auto-generated />
namespace Rowa.Api.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class AddFirstLoginColumntoUserTable : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddFirstLoginColumntoUserTable));
        
        string IMigrationMetadata.Id
        {
            get { return "201804220258174_Add First Login Column to User Table"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}