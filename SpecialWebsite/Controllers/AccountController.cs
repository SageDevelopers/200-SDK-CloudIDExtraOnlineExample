using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RemoteWebsite.Controllers
{
    public class AccountController : Controller
    {
		/// <summary>
		/// Attempts logon to Sage ID
		/// </summary>
		/// <returns>ActionResult of type RedirectResult which redirects to Account/Authorise</returns>
		[RequiresValidLogon]
		public ActionResult LogOn()
		{
			// RequiresValidLogonAttribute will return Unauthorized 401 if not yet logged on,
			// which triggers the Owin logon sequence.
			// If we get into this method, we are already logged on, so just redirect to main page.
			return new RedirectResult(Startup.BaseUri);
		}
    }
}