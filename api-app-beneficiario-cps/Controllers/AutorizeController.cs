
using api_app_beneficiario_cps.App_Code.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace api_app_beneficiario_cps.Controllers
{
    public class AutorizeController : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string username = null;

            if (IsValid(request, out username))
            {
                var principal = new GenericPrincipal(new GenericIdentity(username), null);
                Thread.CurrentPrincipal = principal;

                if (System.Web.HttpContext.Current != null) System.Web.HttpContext.Current.User = principal;

                return base.SendAsync(request, cancellationToken);
            }
            else
            {
                return Task.Factory.StartNew(() =>
                {
                    var r = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    r.Headers.Add("WWW-Authenticate", "Basic realm=\"Necessario informar usuario e senha para acesso a API!\"");
                    return r;
                });
            }
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
