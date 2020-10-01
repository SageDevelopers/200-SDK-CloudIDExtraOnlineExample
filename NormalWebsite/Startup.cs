using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

[assembly: OwinStartup(typeof(NormalWebsite.Startup))]

namespace NormalWebsite
{
	public class Startup
	{

		public const string AccessTokenClaimKey = "http://schemas.sage.com/ws/2012/03/identity/claims/accesstoken";
		public const string AuthType = "SageCloudID";
		public const string BaseUri = @"http://localhost:56953";

        public void Configuration(IAppBuilder app)
		{
			// Configure parameters
			string domain = "id-shadow.sage.com";
			string clientID = "";	// todo "Enter your client ID here";
			string clientSecret = ""; //todo
			string audience = "s200ukish/sage200";

			if (string.IsNullOrEmpty(clientID) || clientID.ToLower().StartsWith("enter"))
			{
				throw new System.ArgumentException("Please edit Startup.cs and specify your Client ID");
			}
			if (string.IsNullOrEmpty(clientSecret) || clientSecret.ToLower().StartsWith("enter"))
			{
				throw new System.ArgumentException("Please edit Startup.cs and specify your Client Secret");
			}

			string redirectUri = BaseUri + "/callback";
			string postLogoutRedirectUri = BaseUri;

			// Enable Kentor Cookie Saver middleware
			app.UseKentorOwinCookieSaver();

			// Set Cookies as default authentication type
			app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
				LoginPath = new PathString("/Account/Login/")
			});

			string authority = "https://" + domain;

			// Configure authentication
			app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
			{
				AuthenticationType = AuthType,

				Authority = authority,

				ClientId = clientID,
				ClientSecret = clientSecret,

				RedirectUri = redirectUri,
				PostLogoutRedirectUri = postLogoutRedirectUri,

				ResponseType = OpenIdConnectResponseType.CodeIdTokenToken,
				Scope = "openid profile email",

				TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					NameClaimType = "name"
				},

				Notifications = new OpenIdConnectAuthenticationNotifications
				{
					SecurityTokenValidated = notification =>
					{
						// Store bearer token within ClaimsPrincipal, so it can be accessed when making API requests
						// See GetAccessToken method in this class
						notification.AuthenticationTicket.Identity.AddClaim(new Claim(AccessTokenClaimKey, notification.ProtocolMessage.AccessToken));

						return Task.FromResult(0);
					},
					RedirectToIdentityProvider = notification =>
					{
						if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
						{
							// Specify audience for bearer token
							notification.ProtocolMessage.SetParameter("audience", audience);
						}
						else if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
						{
							var logoutUri = authority + "/v2/logout?client_id=" + clientID;

							var postLogoutUri = notification.ProtocolMessage.PostLogoutRedirectUri;
							if (!string.IsNullOrEmpty(postLogoutUri))
							{
								if (postLogoutUri.StartsWith("/"))
								{
									// transform to absolute
									var request = notification.Request;
									postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
								}
								postLogoutUri = Uri.EscapeDataString(postLogoutUri);
								logoutUri += "&returnTo=" + postLogoutUri;
							}

							notification.Response.Redirect(logoutUri);
							notification.HandleResponse();
						}
						return Task.FromResult(0);
					}
				}
			});
		}
		
		public static string GetAccessToken()
		{
			string token = string.Empty;

			ClaimsPrincipal principal = ClaimsPrincipal.Current;

			if (principal != null)
			{
				ClaimsIdentity identity = principal.Identities.FirstOrDefault();
				Claim claim = identity.Claims
					.Where(c => c.Type == Startup.AccessTokenClaimKey)
					.FirstOrDefault();

				if (claim != null)
				{
					token = claim.Value;
				}
			}

			return token;
		} 
	}
}
