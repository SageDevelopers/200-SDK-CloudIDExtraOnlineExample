using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Sage.MMS.SAA.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

[assembly: OwinStartup(typeof(RemoteWebsite.Startup))]

namespace RemoteWebsite
{
	public class Startup
	{

		public const string AccessTokenClaimKey = "http://schemas.sage.com/ws/2012/03/identity/claims/accesstoken";
		public const string AuthType = "SageCloudID";
		public const string BaseUri = @"http://localhost:56953";

		private static string _sessionID;

        public void Configuration(IAppBuilder app)
		{

			string token = GetAccessToken();

			// validate  token? 

			List<Company> companies = SAAClientAPI.CompaniesGet();

			Company firstCompany = null;

			foreach (Company company in companies)
			{
				if (firstCompany == null)
				{
					firstCompany = company;		// only for sample logon
				}
			}
			

			// We need to specify a company to use for each subsequent request
			// For the example we just select the first company from the list,
			// however you would more likely display a list of companies and allow
			// the user to select the required company from a list

			SAAClientAPI.ConnectCompany(firstCompany);

			string sessionID = Sage.Common.Contexts.SessionContextValues.SessionID;

			_sessionID = sessionID;
		}

		public static string GetSessionID()
		{
			return _sessionID;
		}
		
		public static string GetAccessToken()
		{
			string token = Sage.Common.IdentityProvider.AuthenticationProvider.GetToken();

			return token;
		} 
	}
}
