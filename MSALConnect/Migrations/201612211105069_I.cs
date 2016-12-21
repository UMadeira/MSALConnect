using System.Data.Entity.Migrations;

namespace MSALConnect.Migrations
{
    public partial class I : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserTokenCaches", newName: "UserTokenCacheEntries");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.UserTokenCacheEntries", newName: "UserTokenCaches");
        }
    }
}
