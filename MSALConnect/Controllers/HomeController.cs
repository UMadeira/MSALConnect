/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Graph;
using MSALConnect.App_GlobalResources;
using MSALConnect.Services;

namespace MSALConnect.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View("Graph");
        }

        // Controller actions

        public ActionResult About()
        {
            return View();
        }

        [Authorize]
        // Get the current user's email address from their profile.
        public async Task<ActionResult> GetMyEmailAddress()
        {
            try
            {
                // Get the current user's email address. 
                ViewBag.Email = await GraphService.Instance.GetMyEmailAddress();
                return View("Graph");
            }
            catch ( ServiceException se )
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        [Authorize]
        // Send mail on behalf of the current user.
        public async Task<ActionResult> SendEmail()
        {
            if ( string.IsNullOrEmpty( Request.Form["email-address"] ) )
            {
                ViewBag.Message = Resource.Graph_SendMail_Message_GetEmailFirst;
                return View("Graph");
            }

            try
            {
                // Build the email message.
                var message = EmailMessageBuilder.Build( 
                    Request.Form["recipients"], Request.Form["subject"], Resource.Graph_SendMail_Body_Content );

                // Send the email.
                await GraphService.Instance.SendEmail( message );

                // Reset the current user's email address and the status to display when the page reloads.
                ViewBag.Email = Request.Form["email-address"];
                ViewBag.Message = Resource.Graph_SendMail_Success_Result;

                return View("Graph");
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
           }
        }

        [Authorize]
        // Get the current user's email address from their profile.
        public async Task<ActionResult> GetPhoto()
        {
            try
            {
                // Get the current user's email address. 
                ViewBag.Stream = await GraphService.Instance.GetPhoto();
                return View( "Photo" );
            }
            catch ( Exception e )
            {
                if ( e.Message == Resource.Error_AuthChallengeNeeded ) return new EmptyResult();
                return RedirectToAction( "Index", "Error", new { message = Resource.Error_Message+Request.RawUrl+": "+e.Message } );
            }
        }
    }
}