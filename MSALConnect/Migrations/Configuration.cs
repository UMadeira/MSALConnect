using System.Data.Entity.Migrations;
using MSALConnect.TokenStorage;

namespace MSALConnect.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<UserTokenCacheDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Microsoft_Graph_SDK_ASPNET_Connect.Models.UserTokenCacheDb";
        }

        protected override void Seed(UserTokenCacheDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
