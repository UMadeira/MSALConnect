﻿using System.Net.Http.Headers;
using Microsoft.Graph;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Helpers
{
    public class SDKHelper
    {
 private static GraphServiceClient graphClient = null;

        // Get an authenticated Microsoft Graph Service client.
        public static GraphServiceClient GetAuthenticatedClient()
        {
            GraphServiceClient graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        string accessToken = await SampleAuthProvider.Instance.GetUserAccessTokenAsync();

                        // Append the access token to the request.
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                    }));
            return graphClient;
        }

        public static void SignOutClient()
        {
            graphClient = null;
        }
    }
}