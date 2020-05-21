using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_app_beneficiario_cps.Models
{
	public class cooperado
	{
		public string nome { get; set; }
		public string lista_qualificacao { get; set; }
		public string descricao { get; set; }
		public string logradouro { get; set; }
		public string numero { get; set; }
		public string complemento { get; set; }
		public string bairro { get; set; }
		public string cep { get; set; }
		public string cidade { get; set; }
		public string sigla { get; set; }
		public string fone { get; set; }
		public string tipo_endereco { get; set; }
		public string cro { get; set; }
		public string link_produto { get; set; }
	}

	public class cooperado_cidade
	{
		public int id_cidade { get; set; }
		public string descricao { get; set; }
		public string sigla { get; set; }
	}

	public class cooperado_especialidade
	{
		public int id_especialidade { get; set; }
		public string descricao { get; set; }
	}

	public class cooperado_bairro
	{
		public int id_endereco { get; set; }
		public string bairro { get; set; }
	}

	public class cooperado_produto_ans
	{
		public int id_plano { get; set; }
		public string descricao { get; set; }
		public string codigo_ans { get; set; }
	}

	public class cooperado_substituto
	{
		public string nome { get; set; }
		public string cro { get; set; }
		public string link_produto { get; set; }
		public string data_fim { get; set; }
		public string data_inicio { get; set; }
	}
}