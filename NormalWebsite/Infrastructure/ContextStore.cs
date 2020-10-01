using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NormalWebsite
{
	/// <summary>
	/// Stores application-specific context eg Site, Company for Sage 200
	/// </summary>
	public static class ContextStore
	{
		/// <summary>
		/// Constant to identify siteID key value for ContextStore SiteID
		/// </summary>
		private const string keySessionID = "ContextStore.SessionID";

		/// <summary>
		/// Move values from Sage 200 SessionContext to HttpContext (for next web request)
		/// </summary>
		/// <param name="session">The session</param>
		/// <param name="siteID">The site ID</param>
		/// <param name="companyID">The company ID</param>
		public static void Set(HttpSessionStateBase session, string sessionID)
		{
			session[keySessionID] = sessionID;
		}

		/// <summary>
		/// Gets the session ID from the HTTPContext as a string
		/// </summary>
		/// <param name="session">The HTTPContext</param>
		/// <returns>The site ID as a string</returns>
		public static string GetSessionID(HttpSessionStateBase session)
		{
			return session[keySessionID] as string;
		}

		/// <summary>
		/// Delete values from HttpContext (for next web request)
		/// </summary>
		/// <param name="session">The HTTPContext</param>
		public static void Reset(HttpSessionStateBase session)
		{
			session[keySessionID] = null;
		}
	}
}