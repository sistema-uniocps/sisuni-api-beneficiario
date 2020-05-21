using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_app_beneficiario_cps.Models
{
	public class user
	{
		public int? id { get; set; }
		public int? beneficiario_id { get; set; }
		public int? id_cooperativa { get; set; }
		public string usuario { get; set; }
		public string email_usuario { get; set; }
		public string perfil { get; set; }
		public string nome_usuario { get; set; }
		public string nome_cooperativa { get; set; }
	}

	public class user_set
	{
		public string nome { get; set; }
		public string cpf { get; set; }
		public string email { get; set; }
		public DateTime data_nascimento { get; set; }
	}

	public class esqueci_senha
	{
		public string email { get; set; }
		public string login { get; set; }
	}
}