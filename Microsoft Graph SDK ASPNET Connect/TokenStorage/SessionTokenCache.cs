/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.Identity.Client;
using Microsoft_Graph_SDK_ASPNET_Connect.Models;

namespace Microsoft_Graph_SDK_ASPNET_Connect.TokenStorage
{

    // Store the user's token information.
    public class SessionTokenCache : TokenCache
    {
        private static readonly object FileLock = new object();
        public string UserUniqueId = string.Empty;

        private UserTokenCacheDb db = new UserTokenCacheDb();
        private UserTokenCache Cache;

        public SessionTokenCache( string aUserUniqueId )
        {
            this.UserUniqueId = aUserUniqueId;

            AfterAccess  = AfterAccessNotification;
            BeforeAccess = BeforeAccessNotification;
            BeforeWrite  = BeforeWriteNotification;

            Load();
        }

        public void Load()
        {
            lock ( FileLock )
            {
                if ( Cache == null )
                {
                    Cache = db.TokenCaches.FirstOrDefault( c => c.UserUniqueId == UserUniqueId );
                }
                else
                {
                    // Retrieve last write from the DB
                    var status = from e in db.TokenCaches
                                 where ( e.UserUniqueId == UserUniqueId )
                                 select new { LastWrite = e.LastWrite };

                    // If the in-memory copy is older than the persistent copy
                    if ( status.First().LastWrite > Cache.LastWrite )
                    {
                        // Read from from storage, update in-memory copy
                        Cache = db.TokenCaches.FirstOrDefault( c => c.UserUniqueId == UserUniqueId );
                    }
                }
                if ( Cache != null )
                {
                    this.Deserialize( MachineKey.Unprotect( Cache.CacheBits, "MSALCache" ) );
                }
            }
        }

        public void Persist()
        {
            if ( ! HasStateChanged ) return;

            lock (FileLock)
            {
                Cache = new UserTokenCache()
                {
                    UserUniqueId = UserUniqueId,
                    CacheBits = MachineKey.Protect( Serialize(), "MSALCache" ),
                    LastWrite = DateTime.Now
                };

                db.Entry( Cache ).State = EntityState.Added;
                db.SaveChanges();

                // After the write operation takes place, restore the HasStateChanged bit to false.
                HasStateChanged = false;
            }
        }

        // Empties the persistent store.
        public override void Clear( string aClientId )
        {
            base.Clear(aClientId);

            var cache = db.TokenCaches.FirstOrDefault( c => c.UserUniqueId == UserUniqueId );
            if ( cache != null )
            {
                db.TokenCaches.Remove( cache );
                db.SaveChanges();
            }
        }

        // Triggered right before ADAL needs to access the cache.
        // Reload the cache from the persistent store in case it changed since the last access.
        private void BeforeAccessNotification( TokenCacheNotificationArgs args )
        {
            Load();
        }

        // Triggered right after ADAL accessed the cache.
        private void AfterAccessNotification( TokenCacheNotificationArgs args )
        {
            // if the access operation resulted in a cache update
            if ( HasStateChanged )
            {
                Persist();
            }
        }
        private void BeforeWriteNotification(TokenCacheNotificationArgs args)
        {
        }
    }
}