using System;
using System.Data.SqlClient;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    public class SqlData
    {
        private static int cmdTimeOut = 1200; 

        /// <summary>
        /// Proc que retorna dados para processo de inclusão no Json
        /// </summary>
        /// <param name="NomeProc"></param>
        /// <returns></returns>
        public static System.Data.DataTable mRetornaDataTable(string strCnx, string NomeProc, ref SqlParameterCollection SqlparamColection)
        { 
            SqlConnection cn = new SqlConnection(strCnx);
            SqlDataReader dr;
            System.Data.DataTable dt = new System.Data.DataTable("dados");
            SqlCommand cm = new SqlCommand(NomeProc, cn);

            if (SqlparamColection != null)
            {
                foreach (SqlParameter param in SqlparamColection)
                {
                    cm.Parameters.AddWithValue(param.ToString(), param.Value);
                }
            }
            cm.CommandType = System.Data.CommandType.StoredProcedure;
            cm.CommandTimeout = cmdTimeOut;

            try
            {
                cn.Open();
                dr = cm.ExecuteReader();
                if (dr.HasRows)
                {
                    dt.Load(dr);
                    dr.Close();
                }
            }
            catch (Exception ex)
            { 
                TratarException.GetErro(ex);
            }
            finally
            {
                if (cn.State == System.Data.ConnectionState.Open) cn.Close();
            }
            return (dt);
        }//mObtemListaRegistro

        /// <summary>
        /// Retorna o número de registros afetados pela query
        /// </summary>
        /// <param name="NomeProc"></param>
        /// <param name="SqlparamColection"></param>
        /// <returns></returns>
        public static int mExecutaCmdSql_ExecuteScalar(string strCnx, string NomeProc, ref SqlParameterCollection SqlparamColection)
        {
            int result = 0;
            
            SqlConnection cn = new SqlConnection(strCnx);
            SqlCommand cm = new SqlCommand(NomeProc, cn);

            if (SqlparamColection != null)
            {
                foreach (System.Data.SqlClient.SqlParameter param in SqlparamColection)
                {
                    cm.Parameters.AddWithValue(param.ToString(), param.Value);
                }
            }
            cm.CommandType = System.Data.CommandType.StoredProcedure;
            cm.CommandTimeout = cmdTimeOut;

            try
            {
                cn.Open();
                object valorObject = cm.ExecuteScalar();
                result = ((valorObject == null) || (valorObject == DBNull.Value)) ? 0 : Convert.ToInt32(valorObject);

            }
            catch (Exception ex)
            {
                TratarException.GetErro(ex);
            }
            finally
            {
                if (cn.State == System.Data.ConnectionState.Open) cn.Close();
            }
            return (result);
        }// mExecutaCmdSql_ExecuteScalar

        /// <summary>
        /// Retorna o número de registros afetados pela query
        /// </summary>
        /// <param name="NomeProc"></param>
        /// <param name="SqlparamColection"></param>
        /// <returns></returns>
        public static int mExecutaCmdSql_ExecuteNonQuery(string strCnx, string NomeProc, ref SqlParameterCollection SqlparamColection)
        {
             int result = 0;
            SqlConnection cn = new SqlConnection(strCnx);
            SqlCommand cm = new SqlCommand(NomeProc, cn);

            if (SqlparamColection != null)
            {
                foreach (System.Data.SqlClient.SqlParameter param in SqlparamColection)
                {
                    cm.Parameters.AddWithValue(param.ToString(), param.Value);
                }
            }
            cm.CommandType = System.Data.CommandType.StoredProcedure;
            cm.CommandTimeout = cmdTimeOut;

            try
            {
                cn.Open();
                object valorObject = cm.ExecuteScalar();
                result = ((valorObject == null) || (valorObject == DBNull.Value)) ? 0 : Convert.ToInt32(valorObject);
            }
            catch (Exception ex)
            {
                TratarException.GetErro(ex);
            }
            finally
            {
                if (cn.State == System.Data.ConnectionState.Open) cn.Close();
            }
            return (result);
        }// mExecutaCmdSql_ExecuteScalar

        /// <summary>
        /// Retorna o resultado de uma query apenas a primeira linha
        /// </summary>
        /// <param name="NomeProc"></param>
        /// <param name="SqlparamColection"></param>
        /// <returns></returns>
        public static string mExecutaCmdSql_ExecuteRow(string strCnx, string NomeProc, ref SqlParameterCollection SqlparamColection)
        { 
            string result = string.Empty;
            SqlConnection cn = new SqlConnection(strCnx);
            SqlCommand cm = new SqlCommand(NomeProc, cn);

            if (SqlparamColection != null)
            {
                foreach (System.Data.SqlClient.SqlParameter param in SqlparamColection)
                {
                    cm.Parameters.AddWithValue(param.ToString(), param.Value);
                }
            }
            cm.CommandType = System.Data.CommandType.StoredProcedure;
            cm.CommandTimeout = cmdTimeOut;
            try
            {
                cn.Open();
                object valorObject = cm.ExecuteScalar();
                result = ((valorObject == null) || (valorObject == DBNull.Value)) ? string.Empty : Convert.ToString(valorObject);

            }
            catch (Exception ex)
            {
                TratarException.GetErro(ex);
            }
            finally
            {
                if (cn.State == System.Data.ConnectionState.Open) cn.Close();
            }
            return (result);
        }// mExecutaCmdSql_ExecuteScalar

        public static Boolean mCarregaGridView(string strCnx,string NomeProc, ref SqlParameterCollection SqlparamColection, ref System.Web.UI.WebControls.GridView gvw)
        { 
            Boolean Result = false;
            SqlConnection cn = new SqlConnection(strCnx);
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
            System.Data.DataSet ds = new System.Data.DataSet();

            SqlCommand cm = new SqlCommand(NomeProc, cn);
            cm.CommandType = System.Data.CommandType.StoredProcedure;

            //Preenche a lista de parâmetros
            if (SqlparamColection != null)
            {
                foreach (System.Data.SqlClient.SqlParameter param in SqlparamColection)
                {
                    cm.Parameters.AddWithValue(param.ToString(), param.Value);
                }
            }
            cm.CommandType = System.Data.CommandType.StoredProcedure;
            cm.CommandTimeout = cmdTimeOut;
            cm.Connection = cn;
            da.SelectCommand = cm;

            try
            {
                da.Fill(ds);
                gvw.DataSource = ds.Tables.Count > 0 ? ds:null;
            }
            catch (Exception ex)
            {
                TratarException.GetErro(ex);
                gvw.DataSource = null;
               
            }
            finally
            {
                gvw.DataBind();
                if (cn.State == System.Data.ConnectionState.Open) cn.Close();
            }
            return (Result);
        }
     }
}