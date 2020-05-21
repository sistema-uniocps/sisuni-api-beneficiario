using Newtonsoft.Json;
using System;
using System.Data.SqlClient;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    /// <summary>
    /// Autor.......: Renato de Ávila Pereira
    /// Criada em...: 06/09/2017
    /// Descrição...: Classe útil para tratar as exceções geradas pelo sistema
    /// </summary>
    public static class TratarException
    {
        /// <summary>
        /// Trata a mensagem de erro e gera log caso seja necessário
        /// </summary>
        /// <param name="e">Exception gerada</param>
        /// <returns>retorna a mensagem tratada</returns>
        public static string GetErro(Exception e, string proc = null, string usuario = null, object dados = null)
        {
            string retorno = string.Empty;
            bool logarErroTexto = true,
                 logarErroEmail = true;

            if (e is CustomException)
            {
                logarErroTexto = false;
                logarErroEmail = false;
                retorno = e.Message;
            }
            else if (e is EnvioEmailException)
            {
                logarErroTexto = false;
                retorno = Mensagem.ErroInesperado;
            }
            else if (e is SqlException)
            {
                SqlException eSqlException = (SqlException)e;
                if (eSqlException.Number == 50000) //RAISERROR('resource.mensagem', 18, 1) RETURN;
                {
                    logarErroTexto = false;
                    logarErroEmail = false;
                    bool encontrouResource = true;
                    retorno = Resource.Get(eSqlException.Message, ref encontrouResource);

                    //Faz o teste se o Resource foi encontrado ou não
                    //Se não tiver sido encontrado manda um alerta para o 
                    //desenvolvedor para que ele corriga a situação
                    if (!encontrouResource)
                    {
                        string mensagem_email = string.Format(
                                   "Olá desenvolvedor<br />" +
                                   "O método api_ouvidoria.App_Code.Utils.TratarException.GetErro está fazendo " +
                                   "uma chamada ao método  api_ouvidoria.App_Code.Utils.Resource.Get, porém, " +
                                   "não está encotrando o Resource {0}<br /> " +
                                   "Foi retornado para o usuário a seguinte mensagem: \"{1}\" <br />" +
                                   "Faça o cadastro da mensagem adequada em  api_ouvidoria.App_Code.Utils.Mensagem <br />" +
                                   "Resource não encontrado:<b>{2}</b><br />" +
                                   "<b>Usuário:</b> {3} <br /><br />" +
                                   "<b>Strored Procedure:</b> {4} <br /><br />" +
                                   "<b>Data/Hora:</b> {5} <br /><br /> " +
                                   "<b>Dados:</b><br /> <pre>{6}</pre><br />" +
                                   "<b>Exception:</b><br /> <pre>{7}</pre>" ,
                                   eSqlException.Message,
                                   retorno,
                                   eSqlException.Message,
                                   usuario ?? "Não identificado.",
                                   proc ?? "Não identificada.",
                                   DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                   JsonConvert.SerializeObject(dados ?? "Não identificado", Formatting.Indented),
                                   JsonConvert.SerializeObject(e, Formatting.Indented)
                               );

                        EmailService.EmailAlertaDesenvolvedor(mensagem_email);
                    }
                }
                else
                {
                    retorno = Mensagem.ErroInesperado;
                }
            }
            else //if(e is Exception) - Esse é o Default
            {
                retorno = retorno = Mensagem.ErroInesperado;
            }

            LogErroService.CriaLog(
                                     logarErroTexto,
                                     logarErroEmail,
                                     e,
                                     proc,
                                     usuario,
                                     dados
            );

            return retorno;
        }

    }
}
