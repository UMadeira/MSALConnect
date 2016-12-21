namespace Microsoft_Graph_SDK_ASPNET_Connect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
