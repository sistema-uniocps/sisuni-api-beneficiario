
using api_app_beneficiario_cps.App_Code.Utils;
using System;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace api_app_beneficiario_cps.Controllers
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class AutorizeGeralAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			bool disableAuthentication = false;

#if DEBUG
			disableAuthentication = true;
#endif

			if (disableAuthentication)
				return true;

			return base.AuthorizeCore(httpContext);
		}

		private static bool IsValid(HttpRequestMessage request, out string username)
		{
			username = null;
			var header = request.Headers.Authorization;

			if (header != null && header.Scheme == "Basic")
			{
				var credentials = header.Parameter;

				if (!string.IsNullOrWhiteSpace(credentials))
				{
					var decodedCredentials =
						Encoding.Default.GetString(Convert.FromBase64String(credentials));

					int separator = decodedCredentials.IndexOf(':');
					var password = decodedCredentials.Substring(separator + 1);
					username = decodedCredentials.Substring(0, separator);

					//Avalia o usuário e senha em relação aos parâmetros do Web Config
					return (
							 username.Equals(AppSetting.http_user_login)
									&& password.Equals(AppSetting.http_user_password) ?
											true : false);
					//Validação do usuário no banco de dados da aplicação
					//return(ObtemDadosUser(username, password));  //Descomentar qdo exisitr a necessidade de validação 
				}
			}
			return false;
		}
	}
}
