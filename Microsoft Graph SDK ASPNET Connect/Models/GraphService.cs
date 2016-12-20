﻿/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Resources;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

// Microsoft.Graph

#if GRAPH

using Microsoft.Graph;

#elif REST

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

#endif

namespace Microsoft_Graph_SDK_ASPNET_Connect.Models
{
    public class GraphService
    {
        #if GRAPH

        // Get the current user's email address from their profile.
        public async Task<string> GetMyEmailAddress(GraphServiceClient graphClient)
        {

            // Get the current user. 
            // The app only needs the user's email address, so select the mail and userPrincipalName properties.
            // If the mail property isn't defined, userPrincipalName should map to the email for all account types. 
            User me = await graphClient.Me.Request().Select("mail,userPrincipalName").GetAsync();
            return me.Mail ?? me.UserPrincipalName;
        }


        // Send an email message from the current user.
        public async Task SendEmail(GraphServiceClient graphClient, Message message)
        {
            await graphClient.Me.SendMail(message, true).Request().PostAsync();
        }

        // Create the email message.
        public Message BuildEmailMessage(string recipients, string subject)
        {

            // Prepare the recipient list.
            string[] splitter = { ";" };
            string[] splitRecipientsString = recipients.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            List<Recipient> recipientList = new List<Recipient>();
            foreach (string recipient in splitRecipientsString)
            {
                recipientList.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = recipient.Trim()
                    }
                });
            }

            // Build the email message.
            Message email = new Message
            {
                Body = new ItemBody
                {
                    Content = Resource.Graph_SendMail_Body_Content,
                    ContentType = BodyType.Html,
                },
                Subject = subject,
                ToRecipients = recipientList
            };
            return email;
        }

        #elif REST

        // Get the current user's email address from their profile.
        public async Task<string> GetMyEmailAddress(string accessToken)
        {

            // Get the current user. 
            // The app only needs the user's email address, so select the mail and userPrincipalName properties.
            // If the mail property isn't defined, userPrincipalName should map to the email for all account types. 
            string endpoint = "https://graph.microsoft.com/v1.0/me";
            string queryParameter = "?$select=mail,userPrincipalName";
            UserInfo me = new UserInfo();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint + queryParameter))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                            me.Address = !string.IsNullOrEmpty(json.GetValue("mail").ToString())?json.GetValue("mail").ToString():json.GetValue("userPrincipalName").ToString();
                        }
                        return me.Address?.Trim();
                    }
                }
            }
        }
        public async Task<byte[]> GetPhoto(string accessToken)
        {
            string endpoint = "https://graph.microsoft.com/v1.0/me/photo/$value";

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage( HttpMethod.Get, endpoint ))
                {
                    request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "image/jpeg" ) );
                    request.Headers.Authorization=new AuthenticationHeaderValue( "Bearer", accessToken );
                    using (HttpResponseMessage response = await client.SendAsync( request ))
                    {
                        if (response.StatusCode==HttpStatusCode.OK)
                        {
                            if (response.Content.Headers.ContentType.MediaType=="image/jpeg")
                            {
                                return await response.Content.ReadAsByteArrayAsync();
                            }
                        }
                    }
                }
            }
            return null;
        }

        public async Task<byte[]> GetUserPhoto( string accessToken, string user)
        {
            string endpoint = $"https://graph.microsoft.com/v1.0/users/{user}/photo/$value";

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage( HttpMethod.Get, endpoint ))
                {
                    request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "image/jpeg" ) );
                    request.Headers.Authorization=new AuthenticationHeaderValue( "Bearer", accessToken );
                    using (HttpResponseMessage response = await client.SendAsync( request ))
                    {
                        if (response.StatusCode==HttpStatusCode.OK)
                        {
                            if (response.Content.Headers.ContentType.MediaType=="image/jpeg")
                            {
                                return await response.Content.ReadAsByteArrayAsync();
                            }
                        }
                    }
                }
            }
            return null;
        }

        // Send an email message from the current user.
        public async Task<string> SendEmail(string accessToken, MessageRequest email)
        {
            string endpoint = "https://graph.microsoft.com/v1.0/me/sendMail";
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    request.Content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return Resource.Graph_SendMail_Success_Result;
                        }
                        return response.ReasonPhrase;
                    }
                }
            }
        }

        // Create the email message.
        public MessageRequest BuildEmailMessage(string recipients, string subject)
        {

            // Prepare the recipient list.
            string[] splitter = { ";" };
            string[] splitRecipientsString = recipients.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            List<Recipient> recipientList = new List<Recipient>();
            foreach (string recipient in splitRecipientsString)
            {
                recipientList.Add(new Recipient
                {
                    EmailAddress = new UserInfo
                    {
                        Address = recipient.Trim()
                    }
                });
            }

            // Build the email message.
            Message message = new Message
            {
                Body = new ItemBody
                {
                    Content = Resource.Graph_SendMail_Body_Content,
                    ContentType = "HTML"
                },
                Subject = subject,
                ToRecipients = recipientList
            };

            return new MessageRequest
            {
                Message = message,
                SaveToSentItems = true
            };
        }

        #endif
    }
}
