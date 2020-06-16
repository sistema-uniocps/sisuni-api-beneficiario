﻿using api_app_beneficiario_cps.App_Code.Utils;
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

	[Authorize]
	public class BeneficiarioController : ApiController, IDisposable
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		LogUtil.LogConfiguracao cfg = new LogConfiguracao(log, System.Web.Hosting.HostingEnvironment.MapPath("~") + AppSetting.DiretorioLog);

		[HttpGet]
		public Retorno<beneficiario> get(int id_pessoa)
		{
			Retorno<beneficiario> retorno;
			DynamicParameters p = new DynamicParameters();

			var _stp = "api_app_beneficiario_pessoa_get";
			var _stp2 = "api_app_beneficiario_pessoa_endereco_get";
			var _stp3 = "api_app_beneficiario_pessoa_plano_get";

			var lista = new List<beneficiario>();
			try
			{
				p.Add("id_pessoa", id_pessoa);

				using (var sqlcon = new SqlConnection(api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql))
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
			DynamicParameters p = new DynamicParameters();

			var _stp = "api_app_beneficiario_lista_dependentes_ativos";
			var id_cooperativa = 5;

			var lista = new List<dependentes_ativos>();
			try
			{
				p.Add("id_cooperativa", id_cooperativa);
				p.Add("id_pessoa_titular", id_pessoa_titular);

				using (var sqlcon = new SqlConnection(api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql))
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
		public Retorno<utilizacao> utilizacao(int id_pessoa)
		{
			Retorno<utilizacao> retorno;
			var _stp = "api_app_beneficiario_utilizacao_plano_get";
			var p = new DynamicParameters();
			var lista = new List<utilizacao>();

			try
			{
				p.Add("id_pessoa", id_pessoa);

				using (var sqlcon = new SqlConnection(api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql))
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

		[HttpGet]
		public Retorno<carteirinha> carteirinha(int id_pessoa)
		{
			Retorno<carteirinha> retorno;
			var _stp = "api_app_beneficiario_carteirinha_get";
			var p = new DynamicParameters();
			var lista = new List<carteirinha>();

			try
			{
				p.Add("id_pessoa", id_pessoa);
				using (var sqlcon = new SqlConnection(api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql))
				{
					lista = sqlcon.Query<carteirinha>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<carteirinha>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<carteirinha>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<carteirinha>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}



	}
}