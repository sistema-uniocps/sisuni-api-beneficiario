using api_app_beneficiario_cps.App_Code.Utils;
using api_app_beneficiario_cps.Models;
using Dapper;
using log4net;
using LogUtil;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace api_app_beneficiario_cps.Controllers
{
    public class LoginController : BaseApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogUtil.LogConfiguracao cfg = new LogConfiguracao(log, System.Web.Hosting.HostingEnvironment.MapPath("~") + AppSetting.DiretorioLog);

		[HttpPost]
		public Retorno<dados_simples> set(user_set user)
		{
			this._cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;

			Retorno<dados_simples> result;
			var _stp = "api_app_beneficiario_login_set";
			var p = new DynamicParameters();
			var lista = new List<dados_login>();

			try
			{
				p.Add("id_cooperativa", 5);
				p.Add("nome_beneficiario", user.nome);
				p.Add("cpf", user.cpf);
				p.Add("email", user.email);
				p.Add("data_nascimento", user.data_nascimento);

				using (var sqlcon = new SqlConnection(this._cnx))
				{
					lista = sqlcon.Query<dados_login>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				if (lista.FirstOrDefault().result > 0)
				{
					var dados_acesso = lista.FirstOrDefault();

					sendEmail(user.email, "Dados de acesso", "Login: " + dados_acesso.login + "<br>" + "Senha: " + dados_acesso.password);
				}

				result = new Retorno<dados_simples>(
													lista.Count > 0 && lista.FirstOrDefault().result > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
													lista.Count > 0 && lista.FirstOrDefault().result > 0 ? string.Empty : Mensagem.NenhumItem,
													lista.FirstOrDefault().result > 0 ?
													null :
													new dados_simples()
													{
														msg = lista.FirstOrDefault().msg,
														result = lista.FirstOrDefault().result
													}
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				result = new Retorno<dados_simples>(
												  HttpStatusCode.InternalServerError,
												  sql.Message
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				result = new Retorno<dados_simples>(
												  HttpStatusCode.InternalServerError,
												  ex.Message
								  );
			}

			return result;
		}

		[HttpPost]
		public Retorno<dados_simples> esqueci_senha(esqueci_senha model)
		{
			this._cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;

			Retorno<dados_simples> result;
			var _stp = "api_usuario_email_get";
			var _stp2 = "api_usuario_password_set";
			int password_set = 0;

			var p = new DynamicParameters();

			var lista = new List<dados_simples>();

			try
			{
				p.Add("pUserName", model.login);
				p.Add("email", model.email);

				using (var sqlcon = new SqlConnection(this._cnx))
				{
					var user = sqlcon.Query<user>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
					if (user.id > 0)
					{
						var user_password = Util.CreatePassword();

						var p2 = new DynamicParameters();
						p2.Add("id_user", user.id);
						p2.Add("pPassword", user_password);

						password_set = sqlcon.Query<int>(_stp2, p2, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

						if (password_set > 0)
						{
							sendEmail(model.email, "Recuperação de senha", "Sua nova senha é " + user_password);
						}
					}
				}

				result = new Retorno<dados_simples>(
													password_set > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													password_set > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				result = new Retorno<dados_simples>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				result = new Retorno<dados_simples>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return result;
		}

		[HttpPost]
		public Retorno<dados_simples> alterar_senha(alterar_senha model)
		{
			this._cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;

			Retorno<dados_simples> result;
			var _stp = "api_usuario_email_get";
			var _stp2 = "api_usuario_password_set";
			int password_set = 0;

			var p = new DynamicParameters();

			var lista = new List<dados_simples>();

			try
			{
				p.Add("pUserName", model.login);
				p.Add("email", model.email);

				using (var sqlcon = new SqlConnection(this._cnx))
				{
					var user = sqlcon.Query<user>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
					if (user.id > 0)
					{
						var p2 = new DynamicParameters();
						p2.Add("id_user", user.id);
						p2.Add("pPassword", model.nova_senha);

						password_set = sqlcon.Query<int>(_stp2, p2, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

						if (password_set > 0)
						{
							sendEmail(model.email, "Alteração de senha", "Alteração de senha realizada com sucesso.");
						}
					}
				}

				result = new Retorno<dados_simples>(
													password_set > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													password_set > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				result = new Retorno<dados_simples>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				result = new Retorno<dados_simples>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return result;
		}
		private void sendEmail(string user_email, string title, string body)
		{
			var dadosSmtp = new DadosSmtp();
			var email = new Email(ref dadosSmtp);
			//var dadosEmail = new DadosEmail(user_email, "Recuperação de senha", "Sua nova senha é " + user_password);
			DadosEmail ml = new DadosEmail();
			ml.Assunto = title;
			ml.Mensagem = ml.MontaCorpo("BENEFICIÁRIO UNIODONTO - ÁREA RESTRITA", title, body, "UNIODONTO DE CAMPINAS");
			ml.EmailDestino = user_email;
			email.Enviar(ml,true);
		}
	}
}