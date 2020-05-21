﻿using System;
using System.Collections.Generic;

namespace api_app_beneficiario_cps.Models
{
	public class beneficiario_titular
	{
		public int id_pessoa { get; set; }
		public string nome { get; set; }
		public string cpf { get; set; }
		public string cns { get; set; }
		public DateTime data_nascimento { get; set; }
		public string email { get; set; }
		public string telefone { get; set; }
		public contrato_plano contrato_plano { get; set; }
		public List<beneficiario_dependente> beneficiario_dependente { get; set; }

		public beneficiario_titular()
		{
			contrato_plano = new contrato_plano();
			beneficiario_dependente = new List<beneficiario_dependente>();
		}
	}
	public class contrato_plano
	{
		public int id_pessoa { get; set; }
		public int id_pessoa_contrato { get; set; }
		public int? id_pessoa_titular { get; set; }
		public int id_grau_dependencia { get; set; }

		public string data_validade_inicio { get; set; }
		public string grau_dependencia { get; set; }

		public int id_contrato { get; set; }
		public int nro_contrato { get; set; }

		public int id_plano { get; set; }
		public string plano { get; set; }
	}

	public class beneficiario_dependente
	{
		public int id_pessoa { get; set; }
		public string nome { get; set; }
		public string cpf { get; set; }
		public string cns { get; set; }
		public string data_nascimento { get; set; }
		public string cod_carteirinha { get; set; }
		public string data_vigencia_inicio { get; set; }
		public string grau_dependencia { get; set; }
	}

	public class dropdown
	{
		public string DataValueField { get; set; }
		public string DataTextField { get; set; }
	}

	public class solicitacao_cancelamento_titular
	{
		public int isCancelarTitular { get; set; }
		public int id_pessoa_contrato_titular { get; set; }
		public int id_motivo_ans { get; set; }
		public string email { get; set; }
		public string ddd { get; set; }
		public string telefone { get; set; }
		public string justificativa_outros { get; set; }
		public List<cancelamento_dependente> dependente { get; set; }

		public solicitacao_cancelamento_titular()
		{
			dependente = new List<cancelamento_dependente>();
		}
	}

	public class cancelamento_dependente
	{
		public int id_pessoa_contrato { get; set; }
		public int id_grau_dependencia { get; set; }
	}


}