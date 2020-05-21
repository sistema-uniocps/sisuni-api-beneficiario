using api_app_beneficiario_cps.App_Code.Utils;
using api_app_beneficiario_cps.Models;
using Dapper;
using log4net;
using LogUtil;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace api_app_beneficiario_cps.Provider
{
	public class OAuthAppProvider : OAuthAuthorizationServerProvider
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		LogUtil.LogConfiguracao cfg = new LogConfiguracao(log, System.Web.Hosting.HostingEnvironment.MapPath("~") + AppSetting.DiretorioLog);

		public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			return Task.Factory.StartNew(() =>
			{
				var username = context.UserName;
				var password = context.Password;
				var _cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;

				user user;
				var _stp = "api_app_beneficiario_login_get";
				var p = new DynamicParameters();

				try
				{
					p.Add("pUserName", username);
					p.Add("pPassword", password);

					using (var sqlcon = new SqlConnection(_cnx))
					{
						user = sqlcon.Query<user>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
					}

					if (user != null)
					{
						var claims = new List<Claim>()
						{
							new Claim("id", user.id.ToString())
						};

						var oAutIdentity = new ClaimsIdentity(claims, Startup.OAuthOptions.AuthenticationType);
						var props = new AuthenticationProperties(
							new Dictionary<string, string>
							{
								{ "nome", user.nome_usuario },
								{ "id_pessoa", user.id.ToString() }
							});
						var ticket = new AuthenticationTicket(oAutIdentity, props);
						context.Validated(ticket);
					}
					else
					{
						context.SetError("login_failed", "Usuário e/ou senha incorretos.");
					}
				}
				catch (SqlException sql)
				{
					log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
					context.SetError("Internal Server Error", "Internal Server Error");
				}
				catch (Exception ex)
				{
					log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
					context.SetError("Internal Server Error", "Internal Server Error");
				}
			});
		}

		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			if (context.ClientId == null)
			{
				context.Validated();
			}
			return Task.FromResult<object>(null);
		}

		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			var authenticationProperties = context.Properties;

			foreach (KeyValuePair<string, string> property in authenticationProperties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);
			}

			return Task.FromResult<object>(null);
		}
	}
}