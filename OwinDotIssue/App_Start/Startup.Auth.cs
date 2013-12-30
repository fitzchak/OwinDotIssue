using System;
using System.Collections.Generic;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using OwinDotIssue.Providers;

namespace OwinDotIssue
{
	public partial class Startup
	{
		static Startup()
		{
			PublicClientId = "self";


			OAuthOptions = new OAuthAuthorizationServerOptions
			{
				TokenEndpointPath = new PathString("/Token"),
				Provider = new ApplicationOAuthProvider(),
				AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
				AllowInsecureHttp = true
			};


		}

		public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }


		public static string PublicClientId { get; private set; }

		// For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
		public void ConfigureAuth(IAppBuilder app)
		{
			app.UseStageMarker(PipelineStage.Authenticate);

			// TODO: Remove the Cookie package
			app.UseCookieAuthentication(new CookieAuthenticationOptions());
			app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ExternalCookie);
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				ExpireTimeSpan = TimeSpan.FromMinutes(5.0),
				CookieName = ".AspNet." + DefaultAuthenticationTypes.ExternalCookie,
				AuthenticationMode = AuthenticationMode.Passive,
				AuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
			});

			// Enable the application to use bearer tokens to authenticate users
			var options = new OAuthAuthorizationServerOptions
			{
				TokenEndpointPath = new PathString("/api/oauth/token"),
				AuthorizeEndpointPath = new PathString("/api/oauth/signin"),
				Provider = new ApplicationOAuthProvider(),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
#if DEBUG || INSECURE
				AllowInsecureHttp = true,
				ApplicationCanDisplayErrors = true,
#endif
			};
			app.UseOAuthAuthorizationServer(options);
			// This middleware only accepts claims where the issuer has been set to LOCAL AUTHORITY.
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
			{
				AccessTokenFormat = options.AccessTokenFormat,
				AccessTokenProvider = options.AccessTokenProvider,
				AuthenticationMode = AuthenticationMode.Active,
				AuthenticationType = options.AuthenticationType,
				Description = options.Description,
				Provider = new ApplicationOAuthBearerProvider(),
				SystemClock = options.SystemClock,
			});
			// Only accepts claims where the issuer is not LOCAL AUTHORITY
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
			{
				AccessTokenFormat = options.AccessTokenFormat,
				AccessTokenProvider = options.AccessTokenProvider,
				AuthenticationMode = AuthenticationMode.Passive,
				AuthenticationType = DefaultAuthenticationTypes.ExternalBearer,
				Description = options.Description,
				Provider = new ApplicationOAuthBearerProvider(),
				SystemClock = options.SystemClock,
			});

			// Uncomment the following lines to enable logging in with third party login providers
			//app.UseMicrosoftAccountAuthentication(
			//    clientId: "",
			//    clientSecret: "");

			//app.UseTwitterAuthentication(
			//    consumerKey: "",
			//    consumerSecret: "");

			//app.UseFacebookAuthentication(
			//    appId: "",
			//    appSecret: "");

			app.UseGoogleAuthentication();

			app.UseStageMarker(PipelineStage.PostAuthenticate);
		}
	}
}
