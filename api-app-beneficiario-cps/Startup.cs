using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(api_app_beneficiario_cps.Startup))]

namespace api_app_beneficiario_cps
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
		}
	}
}