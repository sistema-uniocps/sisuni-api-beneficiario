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

	[Authorize]
	public class CooperadoController : ApiController, IDisposable
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		LogUtil.LogConfiguracao cfg = new LogConfiguracao(log, System.Web.Hosting.HostingEnvironment.MapPath("~") + AppSetting.DiretorioLog);

		[HttpGet]
		public Retorno<cooperado_cidade> cidades_lista_get()
		{
			Retorno<cooperado_cidade> retorno;

			var _stp = "api_app_beneficiario_busca_cooperado_cidades_lista_get";
			var _cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;
			var p = new DynamicParameters();
			var lista = new List<cooperado_cidade>();

			try
			{
				using (var sqlcon = new SqlConnection(_cnx))
				{
					lista = sqlcon.Query<cooperado_cidade>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<cooperado_cidade>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<cooperado_cidade>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<cooperado_cidade>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}

		[HttpGet]
		public Retorno<cooperado_bairro> bairros_lista_get(int id_cidade)
		{
			Retorno<cooperado_bairro> retorno;

			var _stp = "api_app_beneficiario_busca_cooperado_bairro_lista_get";
			var _cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;
			var p = new DynamicParameters();
			var lista = new List<cooperado_bairro>();

			try
			{
				using (var sqlcon = new SqlConnection(_cnx))
				{
					p.Add("id_cidade", id_cidade);

					lista = sqlcon.Query<cooperado_bairro>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<cooperado_bairro>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<cooperado_bairro>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<cooperado_bairro>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}

		[HttpGet]
		public Retorno<cooperado_especialidade> especialidades_lista_get()
		{
			Retorno<cooperado_especialidade> retorno;

			var _stp = "api_app_beneficiario_busca_cooperado_especialidade_lista_get";
			var _cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;
			
			var p = new DynamicParameters();
			var lista = new List<cooperado_especialidade>();

			try
			{
				using (var sqlcon = new SqlConnection(_cnx))
				{
					lista = sqlcon.Query<cooperado_especialidade>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<cooperado_especialidade>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<cooperado_especialidade>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<cooperado_especialidade>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}

		[HttpGet]
		public Retorno<cooperado_produto_ans> produto_ans_lista_get()
		{
			Retorno<cooperado_produto_ans> retorno;

			var _stp = "api_app_beneficiario_busca_cooperado_produto_ans_lista_get";
			var _cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;
			var p = new DynamicParameters();
			var lista = new List<cooperado_produto_ans>();

			try
			{
				using (var sqlcon = new SqlConnection(_cnx))
				{
					lista = sqlcon.Query<cooperado_produto_ans>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<cooperado_produto_ans>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<cooperado_produto_ans>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<cooperado_produto_ans>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}

		[HttpGet]
		public Retorno<cooperado_substituto> credenciado_substituto_lista_get(int id_cidade = 0, int id_especialidade = 0, string bairro = "0", string nome = "0", string cpf = "0")
		{
			Retorno<cooperado_substituto> retorno;

			var _stp = "api_app_beneficiario_busca_cooperado_credenciado_substituto_lista_get";
			var _cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;
			var p = new DynamicParameters();
			var lista = new List<cooperado_substituto>();

			try
			{
				using (var sqlcon = new SqlConnection(_cnx))
				{
					p.Add("id_cidade", id_cidade);
					p.Add("id_especialidade", id_especialidade);
					p.Add("bairro", bairro);
					p.Add("nome", nome);
					p.Add("nome", cpf);

					lista = sqlcon.Query<cooperado_substituto>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<cooperado_substituto>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<cooperado_substituto>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<cooperado_substituto>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}

		[HttpGet]
		public Retorno<cooperado> lista_get(int id_cidade = 0, int id_especialidade = 0, string bairro = "0", string nome = "0")
		{
			Retorno<cooperado> retorno;

			var _stp = "api_app_beneficiario_busca_cooperado_lista_get";
			var _cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;
			var p = new DynamicParameters();
			var lista = new List<cooperado>();

			try
			{
				using (var sqlcon = new SqlConnection(_cnx))
				{
					p.Add("id_cidade", id_cidade);
					p.Add("id_especialidade", id_especialidade);
					p.Add("bairro", bairro);
					p.Add("nome", nome);

					lista = sqlcon.Query<cooperado>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<cooperado>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<cooperado>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<cooperado>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}
	}
}