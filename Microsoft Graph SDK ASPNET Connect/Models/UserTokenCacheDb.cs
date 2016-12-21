namespace Microsoft_Graph_SDK_ASPNET_Connect.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class UserTokenCacheDb : DbContext
    {
        // Your context has been configured to use a 'AuthTokens' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Microsoft_Graph_SDK_ASPNET_Connect.Models.AuthTokens' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AuthTokens' 
        // connection string in the application configuration file.
        public UserTokenCacheDb() : base( "DefaultConnection" )
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<UserTokenCache> TokenCaches { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}