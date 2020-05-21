using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_app_beneficiario_cps.Models
{
	public class gto
	{
		public decimal valor_uso { get; set; }
		public string cod_cartao { get; set; }
		public string nome_beneficiario{ get; set; }
		public string cro { get; set; }
		public string nome_cooperado { get; set; }
		public DateTime data_gto { get; set; }
		public string pd_status_empenho { get; set; }
	}
}