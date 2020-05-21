using api_app_beneficiario_cps.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;

namespace api_app_beneficiario_cps
{
	public partial class Startup
	{
		public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

		static Startup()
		{
			OAuthOptions = new OAuthAuthorizationServerOptions
			{
				TokenEndpointPath = new PathString("/api/login/token"),
				Provider = new OAuthAppProvider(),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
				AllowInsecureHttp = true,
				
			};
		}

		public void ConfigureAuth(IAppBuilder app)
		{
			app.UseOAuthBearerTokens(OAuthOptions);
		}
	}
}