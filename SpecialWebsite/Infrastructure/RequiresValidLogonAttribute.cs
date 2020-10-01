using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RemoteWebsite
{
	/// <summary>
	/// Check the user has logged on
	/// The RequiresValidLogonAttribute attribute validates that user has logged on, 
	/// and redirects to Logon page if not.
	/// Apply this attribute to all controllers where you expect the user to already 
	/// be logged on.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class RequiresValidLogonAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Checks that the user is logged on
		/// Executed on all controllers with RequiresValidLogonAttribute
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

			string token = Startup.GetAccessToken();

			HttpRequestBase request = filterContext.HttpContext.Request;

			try
			{
				// if the token is not set then redirect to the logon page
				if (string.IsNullOrEmpty(token))
				{
					filterContext.Result = new HttpUnauthorizedResult();
				}
			}
			catch(Exception)
			{
				{
					// force re-logon
					//ContextStore.Reset(filterContext.HttpContext.Session);

					filterContext.Result = new System.Web.Mvc.HttpUnauthorizedResult();
				}
			}

		}
	}
}