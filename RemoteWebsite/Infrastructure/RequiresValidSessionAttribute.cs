using Sage.MMS.SAA.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RemoteWebsite.Infrastructure
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class RequiresValidSessionAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Checks that site ID and company ID are set on the session
		/// Executed on all controllers with RequiresValidSiteAttribute
		/// </summary>
		/// <param name="filterContext">The ActionExecutingContext</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			string returnurl = string.Empty;

			try
			{
				// Set the return URL to the request URL
				returnurl = filterContext.HttpContext.Request.Url.AbsolutePath;
			}
			catch
			{
			}

			HttpRequestBase request = filterContext.HttpContext.Request;

			try
			{
				// Check the site ID and Company ID are set on the session, 
				// if not then redirect to the index page

				string sessionID = ContextStore.GetSessionID(filterContext.HttpContext.Session);

				if (string.IsNullOrEmpty(sessionID))
				{
					filterContext.Result = new RedirectToRouteResult(
											new RouteValueDictionary(
												new
												{
													controller = "Sites",
													action = "Index",
													ReturnUrl = returnurl
												}));
				}
				else
				{
					// Use Session to configure SessionContextValues
					SAAClientAPI.SetSessionContext(sessionID);
				}
			}
			catch (Exception)
			{
				{
					// force re-logon
					ContextStore.Reset(filterContext.HttpContext.Session);

					filterContext.Result = new System.Web.Mvc.HttpUnauthorizedResult();
				}
			}
		}
	}
}