using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_app_beneficiario_cps.Models
{
	public class utilizacao
	{
		public int id_gto { get; set; }
		public string nome { get; set; }
		public string cpf { get; set; }
		public string pd_ato { get; set; }
		public decimal valor { get; set; }
		public string data_gto { get; set; }
		public string evento { get; set; }
		public string cooperado_cpf { get; set; }
		public string cooperado_nome { get; set; }
	}
}