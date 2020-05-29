using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Configuration;
using log4net;
using LogUtil;

namespace api_app_beneficiario_cps
{
    class sqlOracle : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogUtil.LogConfiguracao cfg = new LogConfiguracao(log, System.Web.Hosting.HostingEnvironment.MapPath("~") + App_Code.Utils.AppSetting.DiretorioLog);

        System.Data.OleDb.OleDbConnection conn = null;
        string error_;
        string SQL;
        private bool disposed;

        public string ErroOracle
        {
            get { return error_; }
        }


        public sqlOracle()
        {
            error_ = string.Empty;
            SQL = string.Empty;
            disposed = false;

            string sConnString = api_app_beneficiario_cps.Properties.Settings.Default.cnxOracle;
            conn = new System.Data.OleDb.OleDbConnection(sConnString);
        }


        //Implementação da Interface Idipose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            mDesconectaDbOracle();

            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~sqlOracle()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        private Boolean mConectaDbOracle()
        {
            Boolean ret = false;

            try
            {
                conn.Open();
                ret = true;

            }
            catch (Exception ex)
            {
                error_ = ex.Message;
                ///winServiceSisuniDatays.util.mRegistraMensagemArquivo("Erro(mConectaDbOracle())" + error_);
                log.Error("Erro no banco:->" + ex.Message + "\r\n");
            }
            return (ret);
        }//mConectaDbOracle()


        private void mDesconectaDbOracle()
        {
            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
        }//mDesconectaDbOracle()


        public Boolean mExecutacomandoSql(string strSql)
        {
            int resul = 0;
            System.Data.OleDb.OleDbCommand cmd = new OleDbCommand(SQL);
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = strSql;

            if (mConectaDbOracle())
            {
                try
                {
                    resul = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    error_ = ex.Message;
                    log.Error("Erro no banco:->" + strSql + "\r\n" + ex.Message + "\r\n");
                    mDesconectaDbOracle();
                }
            }
            mDesconectaDbOracle();

            if (resul == 0)
                return (false);
            else
                return (true);
        }// mExecutacomandoSql

        public System.Data.DataTable mRetornaDataTable(string strSql)
        {
            System.Data.OleDb.OleDbCommand cmd = new OleDbCommand(strSql, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = strSql;

            System.Data.OleDb.OleDbDataReader dr;
            System.Data.DataTable dt = new System.Data.DataTable("dados");

            if (mConectaDbOracle())
            {
                try
                {
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dt.Load(dr);
                        dr.Close();
                    }
                }
                catch (Exception ex)
                {
                    error_ = ex.Message;
                    log.Error("Erro no banco:->" + strSql + "\r\n" + ex.Message + "\r\n");
                    mDesconectaDbOracle();
                }
                finally
                {
                    mDesconectaDbOracle();
                }
            }
            return (dt);
        }//mVerificaExistenciaRegistroSql

    }//class cnxOracle
}//namespace winServiceSisuniDatays
