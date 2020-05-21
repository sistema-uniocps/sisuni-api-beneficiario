using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_app_beneficiario_cps.Models
{
	public class plano
	{
		public int id_plano { get; set; }
		public int id_contrato { get; set; }
		public string nro_contrato { get; set; }
		public string pd_contrato { get; set; }
		public string pd_plano { get; set; }
		public string cod_cartao { get; set; }
		public string segmentacao { get; set; }
		public string abrangencia { get; set; }
		public string prazo_maximo_carencia { get; set; }
		public string operadora { get; set; }
		public DateTime data_inicio { get; set; }
		public DateTime data_inicio_cobertura { get; set; }
	}
}