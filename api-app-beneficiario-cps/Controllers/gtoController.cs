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
    public class GtoController : BaseApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogUtil.LogConfiguracao cfg = new LogConfiguracao(log, System.Web.Hosting.HostingEnvironment.MapPath("~") + AppSetting.DiretorioLog);

        [HttpGet]
        public Retorno<gto> get(int id_gto)
        {
			this._cnx = api_app_beneficiario_cps.Properties.Settings.Default.cnx_sql;

            Retorno<gto> retorno;
            var _stp = "api_app_beneficiario_gto_get";
            var p = new DynamicParameters();
			var lista = new List<gto>();

            try
            {
                p.Add("id_gto", id_gto);

                using (var sqlcon = new SqlConnection(this._cnx))
                {
                    lista = sqlcon.Query<gto>(_stp, p, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

                retorno = new Retorno<gto>(
                                                    lista.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                                                    lista.Count > 0 ? string.Empty : Mensagem.NenhumItem,
                                                    lista
                                                );
            }
            catch (SqlException sql)
            {

                log.Error("Erro no banco:->" + sql.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");

                retorno = new Retorno<gto>(
                                                  HttpStatusCode.InternalServerError,
                                                  sql.Message,
                                                  lista
                                  );
            }
            catch (Exception ex)
            {
                log.Error("Erro no código:->" + ex.Message + "\r\nConsulta->" + _stp + "\r\n(\r\n" + Util.RetornaDapperParametrosString(p) + ")" + "\r\n");
                retorno = new Retorno<gto>(
                                                  HttpStatusCode.InternalServerError,
                                                  ex.Message,
                                                  lista
                                  );
            }
            return retorno;
        }
	}
}