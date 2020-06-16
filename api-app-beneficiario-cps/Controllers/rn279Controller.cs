using api_app_beneficiario_cps.App_Code.Utils;
using api_app_beneficiario_cps.Models;
using System;

using System.Web.Http;
using log4net;
using LogUtil;
using System.Linq;
using System.Net;
using Dapper;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace api_app_beneficiario_cps.Controllers
{
	[Authorize]
	public class rn279Controller : ApiController, IDisposable
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		LogUtil.LogConfiguracao cfg = new LogConfiguracao(log, System.Web.Hosting.HostingEnvironment.MapPath("~") + AppSetting.DiretorioLog);
		[HttpGet]
		public Retorno<dropdown> listaPlanosAtivosTitularGet(int id_pessoa)
		{
			Retorno<dropdown> retorno;
			var _stp = "api_app_beneficiario_planos_get";
			var p = new DynamicParameters();
			p.Add("id_pessoa", id_pessoa);
			var lista = new List<dropdown>();

			try
			{
				using (var sqlcon = new SqlConnection(api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql))
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

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + "api_app_beneficiario_rn279_titular_dados_get" + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<beneficiario_titular>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  titular
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + "api_app_beneficiario_rn279_titular_dados_get" + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
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
			Retorno<dropdown> retorno;
			var _stp = "api_app_beneficiario_rn279_tipo_cancelamento_get";
			var lista = new List<dropdown>();

			try
			{
				using (var sqlcon = new SqlConnection(api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql))
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

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + "api_app_beneficiario_rn279_processar_cancelamento" + "\r\n(\r\n" + ")" + "\r\n");

				retorno = new Retorno<String>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  protocolo
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + "api_app_beneficiario_rn279_processar_cancelamento" + "\r\n(\r\n" + ")" + "\r\n");
				retorno = new Retorno<String>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  protocolo
								  );
			}
			return retorno;
		}

		[HttpGet]
		public Retorno<cancelamento_movfinanceiro> listaMovimentoFinanceiro( int id_pessoa, int id_tipo_movimento)
		{
			/*
			 * 1- Vencimentos em aberto
			 * 2- Lançamentos Mês
			 * 3- Lançamentos Futuros
			 */
			List<cancelamento_movfinanceiro> lista = new List<cancelamento_movfinanceiro>();
			string strSql = string.Empty;
			strSql += "SELECT";
			strSql += " IDLAN,";
			strSql += " DATAEMISSAO,";
			strSql += " DATAVENCIMENTO,";
			strSql += " NUMERODOCUMENTO,";
			strSql += " VALORORIGINAL ,";
			strSql += " VALORMULTA,";
			strSql += " VALORMORA,";
			strSql += " VALORMULTAMORA,";
			strSql += " VALORTOTAL,";
			strSql += " DATAVENCTO,";
			strSql += " CODCFO";
			strSql += " FROM ";

			string pd_tipo_movimento = string.Empty;
			switch (id_tipo_movimento)
            {
				case 1:
					strSql += " VLANCTOVENCTOABERTO";
					pd_tipo_movimento = "VLANCTOVENCTOABERTO";
					break;
				case 2:
					strSql += " VLANCTOVENCTOMES";
					pd_tipo_movimento = "VLANCTOVENCTOMES";
					break;
				case 3:
					strSql += " VLANCTOVENCTOFUTURO";
					pd_tipo_movimento = "VLANCTOVENCTOFUTURO";
					break;
            }
			strSql += " WHERE ";

			string formatCod = ("000000000" + id_pessoa.ToString());
			formatCod = "'5" + formatCod.Substring(formatCod.Length - 9, 9) + "'";
			strSql += " CODCFO = " + formatCod;
			strSql += " ORDER BY DATAVENCTO ASC ";

			sqlOracle cnxOra = new sqlOracle();
			DataTable dt = cnxOra.mRetornaDataTable(strSql);

            foreach (DataRow dr in dt.Rows)
            {
				cancelamento_movfinanceiro item = new cancelamento_movfinanceiro();
				item.id_pessoa = id_pessoa.ToString();
				item.id_tipo_movimento = id_tipo_movimento.ToString();
				item.pd_tipo_movimento = pd_tipo_movimento;

				item.IDLAN = dr["IDLAN"].ToString();
				item.DATAEMISSAO = dr["DATAEMISSAO"].ToString();
				item.DATAVENCIMENTO = dr["DATAVENCIMENTO"].ToString();
				item.NUMERODOCUMENTO = dr["NUMERODOCUMENTO"].ToString();
				item.VALORORIGINAL = dr["VALORORIGINAL"].ToString();
				item.VALORMULTA = dr["VALORMULTA"].ToString();
				item.VALORMORA = dr["VALORMORA"].ToString();
				item.VALORMULTAMORA = dr["VALORMULTAMORA"].ToString();
				item.VALORTOTAL = dr["VALORTOTAL"].ToString();
				item.DATAVENCTO = dr["DATAVENCTO"].ToString();
				item.CODCFO = dr["CODCFO"].ToString();

				lista.Add(item);
			}
            Retorno<cancelamento_movfinanceiro> retorno;
			retorno = new Retorno<cancelamento_movfinanceiro>(
									lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
									lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
									lista
								);

			return retorno;
		}

	}

}