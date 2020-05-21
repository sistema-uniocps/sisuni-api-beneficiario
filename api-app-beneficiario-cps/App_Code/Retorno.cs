using System;
using System.Collections.Generic;
using System.Net;

namespace api_app_beneficiario_cps
{
    /// <summary>
    /// Estrutura padrão para resposta de execução do métodos api
    /// </summary>
    [Serializable]
    public class Retorno<T>
    {
        public HttpStatusCode http_status_cod { get; set; }
        public string http_status_cod_desc { get; set; }
        public string msg { get; set; }
        public string num_reg_data { get; set; }
        public List<T> data { get; set; }

        public Retorno(HttpStatusCode http_status_cod, string msg, List<T> data = null)
        {
            this.http_status_cod = http_status_cod;
            this.http_status_cod_desc = http_status_cod.ToString();
            this.msg = msg;
            if (data != null && data.Count > 0)
            {
                this.data = data;
                this.num_reg_data = data.Count.ToString();
            }
            else
            {
                this.data = new List<T> { (default(T) == null) ? Activator.CreateInstance<T>() : default(T) };
            };
        }

        public Retorno(HttpStatusCode http_status_cod, string msg, T data)
        {
            this.http_status_cod = http_status_cod;
            this.http_status_cod_desc = http_status_cod.ToString();
            this.msg = msg;
            if (data != null)
            {
                this.data = new List<T>();
                this.data.Add(data);
                this.num_reg_data = "1";
            }
            else
            {
                this.data = new List<T> { (default(T) == null) ? Activator.CreateInstance<T>() : default(T)
            };
            }
        }
    }

}

