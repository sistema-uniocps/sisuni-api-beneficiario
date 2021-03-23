using api_app_beneficiario_cps.App_Code.Utils;
using api_app_beneficiario_cps.Models;
using Dapper;
using log4net;
using LogUtil;
using Microsoft.Owin.Security.OAuth;
using rptToPdf.objReporting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

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

		#region [Relatórios/Contrato - Atende Fim contrato YPE]
		/*
		 * Método implementado para atender a uma demanda da YPÊ com fim de contrato
		 * 26/01/2020
		 * 
		 */ 
		[ResponseType(typeof(Retorno<reportOut>))]
		[HttpGet]
		public Retorno<reportOut> contratopf_get(string primeiro_nome, int id_pessoa, int isExcluirDependente)
		{
			Retorno<reportOut> retorno;
			reportOut obj = new reportOut();
			//--------------------------------------- Nome do RPT/Contato

			//--------------------------------------- DEFINE O TIPO
			rptToPdf.objReporting.RptToPdf.tipoSaidaReport tipoRpt = new RptToPdf.tipoSaidaReport();
			tipoRpt = RptToPdf.tipoSaidaReport.PDF;

			//--------------------------------------- OBTEM OS PARÂMETROS
			System.Collections.Hashtable parametros = new System.Collections.Hashtable();
			parametros.Add("id_pessoa"/*Nome*/, id_pessoa/*valor*/);
			parametros.Add("isexcluirdependente"/*Nome*/, isExcluirDependente /*valor*/);

			//--------------------------------------- GERA O RELATÓRIO
			try
			{
				byte[] arqBinario = mRetornaBytesPdf_Report(
																5/*id_cooperativa*/,
																"rpt_contrato_YPE",
																parametros,
																tipoRpt
														   );
				obj.documento_x64 = Convert.ToBase64String(arqBinario.ToArray());
				obj.documento_nome = "contrato-pf-" + primeiro_nome + ".pdf";

				if (!string.IsNullOrWhiteSpace(obj.documento_x64))
				{
					retorno = new Retorno<reportOut>(
														HttpStatusCode.OK,
														string.Empty,
														obj
													);
				}
				else
				{
					retorno = new Retorno<reportOut>(
														HttpStatusCode.NotFound,
														"A execução do relatório não retornou registos.",
														obj
													);

				}
			}
			catch (Exception ex)
			{
				log.Error("Erro c#:->" + ex.Message);
				retorno = new Retorno<reportOut>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  obj
								  );
			}
			return retorno;
		}

		[ResponseType(typeof(Retorno<reportOut>))]
		[HttpGet]
		public Retorno<reportOut> contratopf_padrao_get(
														string primeiro_nome,
														int id_pessoa,
														int nro_contrato_origem, 
														int nro_contrato_destino, 
														int isExcluirDependente
			)
		{
			Retorno<reportOut> retorno;
			reportOut obj = new reportOut();
			//--------------------------------------- Nome do RPT/Contato

			//--------------------------------------- DEFINE O TIPO
			rptToPdf.objReporting.RptToPdf.tipoSaidaReport tipoRpt = new RptToPdf.tipoSaidaReport();
			tipoRpt = RptToPdf.tipoSaidaReport.PDF;

			//--------------------------------------- OBTEM OS PARÂMETROS
			System.Collections.Hashtable parametros = new System.Collections.Hashtable();
			parametros.Add("id_pessoa", id_pessoa);
			parametros.Add("nro_contrato_origem", nro_contrato_origem);
			parametros.Add("nro_contrato_destino", nro_contrato_destino);
			parametros.Add("isexcluirdependente", isExcluirDependente) ;

			//--------------------------------------- GERA O RELATÓRIO
			try
			{
				byte[] arqBinario = mRetornaBytesPdf_Report(
																5/*id_cooperativa*/,
																"rpt_contrato_pj_to_pf",
																parametros,
																tipoRpt
														   );
				obj.documento_x64 = Convert.ToBase64String(arqBinario.ToArray());
				obj.documento_nome = "contrato-pf-" + primeiro_nome + ".pdf";

				if (!string.IsNullOrWhiteSpace(obj.documento_x64))
				{
					retorno = new Retorno<reportOut>(
														HttpStatusCode.OK,
														string.Empty,
														obj
													);
				}
				else
				{
					retorno = new Retorno<reportOut>(
														HttpStatusCode.NotFound,
														"A execução do relatório não retornou registos.",
														obj
													);

				}
			}
			catch (Exception ex)
			{
				log.Error("Erro c#:->" + ex.Message);
				retorno = new Retorno<reportOut>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  obj
								  );
			}
			return retorno;
		}




		private byte[] mRetornaBytesPdf_Report(
													   int id_cooperativa,
													   string nomeRpt,
													   System.Collections.Hashtable paramRpt,
													   rptToPdf.objReporting.RptToPdf.tipoSaidaReport tipo
												   )
		{
			/*
             *    
                    Hashtable paramRpt = new Hashtable();
                    string nomeRpt = "rpt_sis_carta_notificacao_reajuste_pj";
                    //---------------------------------------------------------- Obtem o binário do report
                    paramRpt.Add("id_simulador_header", id_simulador_header);
                    paramRpt.Add("id_simulador_item", id_simulador_item);
             */
			string DirLog = System.Web.Hosting.HostingEnvironment.MapPath("~") + "\\" + "logRpt";

			byte[] arrayBytesRetornados = null;
			RptToPdf objRpt = new RptToPdf();

			string urlServicoRpt = api_app_beneficiario_cps.Properties.Settings.Default.urlReportServices;
			string ReportFolder = api_app_beneficiario_cps.Properties.Settings.Default.folderRpt;

			System.Net.NetworkCredential cred = new System.Net.NetworkCredential();
			cred.UserName = api_app_beneficiario_cps.Properties.Settings.Default.usrRpt;
			cred.Password = api_app_beneficiario_cps.Properties.Settings.Default.senhaRpt;
			cred.Domain = api_app_beneficiario_cps.Properties.Settings.Default.dominioRpt; 

			arrayBytesRetornados = objRpt.RetornaBytesReportAnalitico(cred, urlServicoRpt, ReportFolder, nomeRpt, paramRpt, DirLog, tipo);

			return (arrayBytesRetornados);
		}

		[HttpPost, HttpPut]
		[ResponseType(typeof(Retorno<dados_simples>))]
		public Retorno<dados_simples> contratopf_set(arquivoIn contratoPf)
		{
			Retorno<dados_simples> retorno;
			DynamicParameters p = new DynamicParameters();
			dados_simples obj = new dados_simples();
			string _stp = "datasys_pessoa_documento_set";

			try
			{
				p.Add("id_pessoa", contratoPf.id_pessoa );
				p.Add("id_tipo_documento", contratoPf.id_tipo_documento);
				p.Add("documento_nome", contratoPf.documento_nome);
				p.Add("documento_extensao", contratoPf.documento_extensao);
				p.Add("binario", Convert.FromBase64String(contratoPf.documento_x64) );

				using (var sqlcon = new SqlConnection(api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql))
				{
					obj = sqlcon.Query<dados_simples>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
				}

				retorno = new Retorno<dados_simples>(
													obj.result == 1 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
													obj.result == 1 ? string.Empty : "Erro inesperado no processo de registro, tente novamente em 10 minutos.",
													obj
												);
				//Pessoa/Beneficiário não foi identificado
				if (obj.result == 0)
				{
					retorno = new Retorno<dados_simples>(
													  HttpStatusCode.InternalServerError,
													  "Erro no processo de validação do E-mail ",
													  obj
									  );
				}
			}
			catch (SqlException sql)
			{

				log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<dados_simples>(
												  HttpStatusCode.InternalServerError,
												  sql.Message,
												  obj
								  );
			}
			catch (Exception ex)
			{
				log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

				retorno = new Retorno<dados_simples>(
												  HttpStatusCode.InternalServerError,
												  ex.Message,
												  obj
								  );
			}
			return retorno;
		}


		#endregion


	}
}