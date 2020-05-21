using api_app_beneficiario_cps.App_Code.Utils;
using api_app_beneficiario_cps.Models;
using Dapper;
using log4net;
using LogUtil;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace api_app_beneficiario_cps.Controllers
{
	
	public class BeneficiarioController : BaseApiController
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		LogUtil.LogConfiguracao cfg = new LogConfiguracao(log, System.Web.Hosting.HostingEnvironment.MapPath("~") + AppSetting.DiretorioLog);

		[AutorizeGeral]
		[HttpGet]
		public Retorno<beneficiario> get()
		{
			Retorno<beneficiario> retorno;

			var _stp = "api_app_beneficiario_pessoa_get";
			var _stp2 = "api_app_beneficiario_pessoa_endereco_get";
			var _stp3 = "api_app_beneficiario_pessoa_plano_get";

			this._cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;
			
			DynamicParameters p = new DynamicParameters();

			var lista = new List<beneficiario>();
			try
			{
				p.Add("id_pessoa", this._id);

				using (var sqlcon = new SqlConnection(this._cnx))
				{
					lista = sqlcon.Query<beneficiario>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();

					lista.ForEach(b =>
					{
						b.endereco = sqlcon.Query<endereco>(_stp2, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
						b.plano = sqlcon.Query<plano>(_stp3, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
					});
				}

				retorno = new Retorno<beneficiario>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<beneficiario>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<beneficiario>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}

		[HttpGet]
		public Retorno<dependentes_ativos> dependentes_ativos(int id_pessoa_titular)
		{
			Retorno<dependentes_ativos> retorno;

			var _stp = "api_app_beneficiario_lista_dependentes_ativos";
			var id_cooperativa = 5;

			this._cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;

			DynamicParameters p = new DynamicParameters();

			var lista = new List<dependentes_ativos>();
			try
			{
				p.Add("id_cooperativa", id_cooperativa);
				p.Add("id_pessoa_titular", id_pessoa_titular);

				using (var sqlcon = new SqlConnection(this._cnx))
				{
					lista = sqlcon.Query<dependentes_ativos>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<dependentes_ativos>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<dependentes_ativos>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<dependentes_ativos>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}

		[HttpGet]
		public Retorno<utilizacao> utilizacao()
		{
			this._cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;

			Retorno<utilizacao> retorno;
			var _stp = "api_app_beneficiario_utilizacao_plano_get";
			var p = new DynamicParameters();
			var lista = new List<utilizacao>();

			try
			{
				p.Add("id_pessoa", this._id);

				using (var sqlcon = new SqlConnection(this._cnx))
				{
					lista = sqlcon.Query<utilizacao>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<utilizacao>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<utilizacao>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<utilizacao>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}
	}
}