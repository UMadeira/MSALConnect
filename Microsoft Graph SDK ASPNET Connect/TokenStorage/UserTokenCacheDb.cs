using System.Data.Entity;

namespace Microsoft_Graph_SDK_ASPNET_Connect.TokenStorage
{
    public class UserTokenCacheDb : DbContext
    {
        public UserTokenCacheDb() : base( "name=TokensCacheConnection" )
        {
        }

        public virtual DbSet<UserTokenCacheEntry> TokenCaches { get; set; }
    }
}