/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft_Graph_SDK_ASPNET_Connect.TokenStorage;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Resources;

using Microsoft.Graph;
#if GRAPH
#endif

namespace Microsoft_Graph_SDK_ASPNET_Connect.Helpers
{
    public sealed class SampleAuthProvider : IAuthProvider
    {

        // Properties used to get and manage an access token.
        private string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private string appId = ConfigurationManager.AppSettings["ida:AppId"];
        private string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
        private string scopes = ConfigurationManager.AppSettings["ida:GraphScopes"];
        private SessionTokenCache TokenCache { get; set; }

        private static readonly SampleAuthProvider instance = new SampleAuthProvider();
        private SampleAuthProvider() { } 

        public static SampleAuthProvider Instance
        {
            get
            {
                return instance;
            }
        }

        // Gets an access token. First tries to get the token from the token cache.
        public async Task<string> GetUserAccessTokenAsync()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

            TokenCache = new SessionTokenCache( signedInUserID );

            #if DEBUG
            var cachedItems = TokenCache.ReadItems(appId); // see what's in the cache
            foreach ( var item in cachedItems )
            {
                var token = item as TokenCacheItem;
                if ( token == null ) continue;

                System.Diagnostics.Trace.WriteLine( "-----------------------------------------------------------------------" );
                System.Diagnostics.Trace.WriteLine( string.Format( "    Authority : {0}", token.Authority ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "     TenantId : {0}", token.TenantId ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "     ClientId : {0}", token.ClientId ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "     UniqueId : {0}", token.UniqueId ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "DisplayableId : {0}", token.DisplayableId ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "         Name : {0}", token.Name ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "       Scopes : {0}", string.Join( ", ", token.Scope ) ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "    ExpiresOn : {0}", token.ExpiresOn ) );
                System.Diagnostics.Trace.WriteLine( "" );
                System.Diagnostics.Trace.WriteLine( string.Format( " Token => {0}", token.Token ) );
                System.Diagnostics.Trace.WriteLine( "" );
            }
            #endif

            ConfidentialClientApplication cca = new ConfidentialClientApplication(
                appId, redirectUri, new ClientCredential(appSecret), TokenCache 
            );

            try
            {
                AuthenticationResult result = await cca.AcquireTokenSilentAsync(scopes.Split(new char[] { ' ' }));
                return result.Token;
            }

            // Unable to retrieve the access token silently.
            catch ( MsalSilentTokenAcquisitionException )
            {
                #if GRAPH
                    throw new ServiceException( new Error {
                            Code = GraphErrorCode.AuthenticationFailure.ToString(),
                            Message = Resource.Error_AuthChallengeNeeded,
                        });
                #elif REST
                    HttpContext.Current.Request.GetOwinContext().Authentication.Challenge(
                        new AuthenticationProperties() { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);

                    throw new Exception(Resource.Error_AuthChallengeNeeded);
                #endif
            }
        }
    }
}
