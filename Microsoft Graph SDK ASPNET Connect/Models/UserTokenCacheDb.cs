namespace Microsoft_Graph_SDK_ASPNET_Connect.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class UserTokenCacheDb : DbContext
    {
        // If you wish to target a different database and/or database provider, modify the 'AuthTokens' 
        // connection string in the application configuration file.
        public UserTokenCacheDb() : base( "name=TokensCacheConnection" )
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<UserTokenCache> TokenCaches { get; set; }
    }
}