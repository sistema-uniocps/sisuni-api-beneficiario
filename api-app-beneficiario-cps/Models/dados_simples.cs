using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_app_beneficiario_cps.Models
{
	[Serializable]
	public class dados_simples
	{
		public int result { get; set; }
		public string msg { get; set; }
	}

	[Serializable]
	public class dados_login
	{
		public int result { get; set; }
		public string msg { get; set; }
		public string login { get; set; }
		public string password { get; set; }
	}
}