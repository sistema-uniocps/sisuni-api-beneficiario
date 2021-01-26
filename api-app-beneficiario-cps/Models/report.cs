using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_app_beneficiario_cps.Models
{
    public class arquivoIn
    {
        public int id_pessoa { get; set; }
        public int id_tipo_documento { get; set; }
        public string documento_nome { get; set; }
        public string documento_extensao { get; set; }
        public string documento_x64 { get; set; }
    }
    [Serializable]
    public class paramRpt
    {
        public string parametro_nome { get; set; }
        public string parametro_valor { get; set; }
    }

    public class reportOut
    {
        public string documento_nome { get; set; }
        public string doucumento_extensao { get; set; }
        public string documento_x64 { get; set; }

    }
}