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
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPutAttribute = System.Web.Mvc.HttpPutAttribute;

namespace api_app_beneficiario_cps.Controllers
{

	public class rn279Controller : BaseApiController
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		LogUtil.LogConfiguracao cfg = new LogConfiguracao(log, System.Web.Hosting.HostingEnvironment.MapPath("~") + AppSetting.DiretorioLog);
		[AutorizeGeral]
		[HttpGet]
		public Retorno<dropdown> listaPlanosAtivosTitularGet(int id_pessoa)
		{
			this._cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;

			Retorno<dropdown> retorno;
			var _stp = "api_app_beneficiario_planos_get";
			var p = new DynamicParameters();
			p.Add("id_pessoa", id_pessoa);
			var lista = new List<dropdown>();

			try
			{
				using (var sqlcon = new SqlConnection(this._cnx))
				{
					lista = sqlcon.Query<dropdown>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<dropdown>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<dropdown>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<dropdown>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}
		[HttpGet]
		public Retorno<beneficiario_titular> dadosTitularDependenteGet(int id_pessoa, int id_plano)
		{
			Retorno<beneficiario_titular> retorno;
			beneficiario_titular titular = new beneficiario_titular();

			DynamicParameters p = new DynamicParameters();
			DynamicParameters p2 = new DynamicParameters();
			try
			{
				using (var sqlcon = new SqlConnection(api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql))
				{
					p.Add("id_pessoa", id_pessoa);
					p.Add("id_plano", id_plano);
					titular = sqlcon.Query<beneficiario_titular>(
																	"api_app_beneficiario_rn279_titular_dados_get",
																	p,
																	commandType: System.Data.CommandType.StoredProcedure
																).FirstOrDefault();

					if (titular != null)
					{
						titular.contrato_plano = sqlcon.Query<contrato_plano>(
																				"api_app_beneficiario_rn279_titular_contrato_plano_get",
																				p,
																				commandType: System.Data.CommandType.StoredProcedure
																			).FirstOrDefault();

						p2.Add("id_pessoa_contrato_titular", titular.contrato_plano.id_pessoa_contrato);
						titular.beneficiario_dependente = sqlcon.Query<beneficiario_dependente>(
																				"api_app_beneficiario_rn279_titular_dependente_get",
																				p2,
																				commandType: System.Data.CommandType.StoredProcedure
																			).ToList();
					}
				}

				retorno = new Retorno<beneficiario_titular>(
													titular != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													titular != null ? string.Empty : Mensagem.NenhumItem,
													titular
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<beneficiario_titular>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  titular
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
				retorno = new Retorno<beneficiario_titular>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  titular
								  );
			}
			return retorno;
		}
		[HttpGet]
		public Retorno<dropdown> listaMotivosCancelamentoAnsGet()
		{
			this._cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;

			Retorno<dropdown> retorno;
			var _stp = "api_app_beneficiario_rn279_tipo_cancelamento_get";
			var lista = new List<dropdown>();

			try
			{
				using (var sqlcon = new SqlConnection(this._cnx))
				{
					lista = sqlcon.Query<dropdown>(_stp, null, commandType: System.Data.CommandType.StoredProcedure).ToList();
				}

				retorno = new Retorno<dropdown>(
													lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
													lista
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + ")" + "\r\n");

				retorno = new Retorno<dropdown>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  lista
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + ")" + "\r\n");
				retorno = new Retorno<dropdown>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  lista
								  );
			}
			return retorno;
		}

		[HttpPut]
		public Retorno<string> processarCancelamentoSet(solicitacao_cancelamento_titular obj)
		{
			Retorno<string> retorno;
			string protocolo = string.Empty;
			int id_solicitacao_cancelamento = 0;
			try
			{
				DynamicParameters p = new DynamicParameters();
				p.Add("id_pessoa_contrato_titular", obj.id_pessoa_contrato_titular);
				p.Add("id_motivo_ans", obj.id_motivo_ans);
				p.Add("isCancelarTitular", obj.isCancelarTitular);
				p.Add("email", obj.email);
				p.Add("ddd", obj.ddd);
				p.Add("telefone", obj.telefone);
				p.Add("justificativa_outros", obj.justificativa_outros);


				using (var sqlcon = new SqlConnection(api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql))
				{
					//--------------------------- registra a solicitação
					id_solicitacao_cancelamento = sqlcon.Query<Int32>(
																		"api_app_beneficiario_rn279_solicitacao_titular_set",
																		p,
																		commandType: System.Data.CommandType.StoredProcedure
																	).FirstOrDefault();
					//--------------------------- dependentes do titular
					foreach (cancelamento_dependente item in obj.dependente)
					{
						DynamicParameters p2 = new DynamicParameters();
						p2.Add("id_solicitacao_cancelamento", id_solicitacao_cancelamento);
						p2.Add("id_pessoa_contrato", item.id_pessoa_contrato);
						sqlcon.Query<Int32>(
												"api_app_beneficiario_rn279_solicitacao_depen_set",
												p2,
												commandType: System.Data.CommandType.StoredProcedure
											).FirstOrDefault();

					}
					//------------------------- processa o cancelamento
					DynamicParameters p3 = new DynamicParameters();
					p3.Add("id_solicitacao_cancelamento", id_solicitacao_cancelamento);
					protocolo = sqlcon.Query<String>(
														"api_app_beneficiario_rn279_processar_cancelamento",
														p3,
														commandType: System.Data.CommandType.StoredProcedure
													).FirstOrDefault();
				}

				retorno = new Retorno<String>(
													!string.IsNullOrWhiteSpace(protocolo) ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													!string.IsNullOrWhiteSpace(protocolo) ? string.Empty : Mensagem.NenhumItem,
													protocolo
												);
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + ")" + "\r\n");

				retorno = new Retorno<String>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  protocolo
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + ")" + "\r\n");
				retorno = new Retorno<String>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  protocolo
								  );
			}
			return retorno;
		}
	}
}