using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_app_beneficiario_cps.Models
{
    public class beneficiario
    {
        public int id_pessoa { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string cns { get; set; }
        public DateTime data_nascimento { get; set; }
        public string cod_carteirinha { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public List<plano> plano { get; set; }
        public List<endereco> endereco { get; set; }
    }

    public class dependentes_ativos
    {
        public int id_pessoa { get; set; }
        public int id_pessoa_contrato { get; set; }
        public int id_pessoa_titular { get; set; }
        public string codigo_cartao { get; set; }
        public string nome { get; set; }
        public string data_validade_inicio { get; set; }
        public string data_validade_fim { get; set; }
    }
}