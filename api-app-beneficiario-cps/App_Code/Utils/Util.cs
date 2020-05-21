using System;
using System.Text;
using Dapper;


namespace api_app_beneficiario_cps.App_Code.Utils
{
    public static class Util
    {



        /// <summary>
        /// Retorna "length" caracteres a esquerda de uma String
        /// </summary>
        /// <param name="valor">String a ser trabalhada</param>
        /// <param name="length">Quantidade de caracteres</param>
        /// <returns>Retorna "length" caracteres a esquerda de uma String</returns>
        public static string Left(string valor, int length)
        {
            if (valor.Length < length)
            {
                length = valor.Length;
            }
            return valor.Substring(0, length);
        }

        /// <summary>
        /// Retorna "length" caracteres a direita de uma String
        /// </summary>
        /// <param name="valor">String a ser trabalhada</param>
        /// <param name="length">Quantidade de caracteres</param>
        /// <returns>Retorna "length" caracteres a direita de uma String</returns>
        public static string Right(string valor, int length)
        {
            if (valor.Length < length)
            {
                length = valor.Length;
            }
            return valor.Substring(0, length);

        }

        #region [Trata parametros de Dapper]

        public static string RetornaDapperParametrosString(DynamicParameters p)
        {
            string retorno = "";

            try
            {
                var sb = new StringBuilder();
                foreach (var nome in p.ParameterNames)
                {
                    var valor = p.Get<dynamic>(nome);
                    sb.AppendFormat("{0}={1} \r\n", nome, valor.ToString());
                }

                retorno = sb.ToString();
            }
            catch (System.Exception)
            {
                throw; 
            }

            return retorno;

        }
        #endregion

        public static  string CreatePassword()
        {
            int tamanho = api_app_beneficiario_cps.Properties.Settings.Default.numdigitosSenha;
            string SenhaCaracteresValidos = api_app_beneficiario_cps.Properties.Settings.Default.caracteresSenha;

            int valormaximo = SenhaCaracteresValidos.Length;

            Random random = new Random(DateTime.Now.Millisecond);

            System.Text.StringBuilder senha = new System.Text.StringBuilder(tamanho);

            for (int indice = 0; indice < tamanho; indice++)
            {
                senha.Append(SenhaCaracteresValidos[random.Next(0, valormaximo)]);
            }

            return senha.ToString().ToUpper();
        }

    }
}
