using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_app_beneficiario_cps.Models
{
	public class endereco
	{
		public string cep { get; set; }
		public string pd_tipo_logradouro { get; set; }
		public string logradouro { get; set; }
		public string numero { get; set; }
		public string complemento { get; set; }
		public string pd_cidade { get; set; }
		public string pd_bairro { get; set; }
		public string pd_uf { get; set; }
		public string pd_tipo_endereco { get; set; }
	}
}