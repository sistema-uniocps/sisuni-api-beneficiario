using api_app_beneficiario_cps.App_Code.Utils;
using Newtonsoft.Json;
using System;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    /// <summary>
    /// Autor.......: Renato de Ávila Pereira
    /// Criada em...: 06/06/2018
    /// Descrição...: Serviço de geração de aquivos
    /// ............. Essa classe usa a [Class Arquivo] para geração
    /// </summary>
    public class ArquivoService
    {
        /// <summary>
        /// Cria Log de erro em aquivo, lembre-se de configurar AppSetting.DiretorioLog
        /// </summary>
        /// <param name="e">Exceção gerada pelo sistema</param>
        /// <param name="proc">Stored Procedure</param>
        /// <param name="usuario">Usuário do sistema</param>
        /// <param name="dados">Dados no momento da exceção</param>
        public static void ArquivoLogErro(Exception e, string proc = null, string usuario = null, object dados = null)
        {
            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~") + AppSetting.DiretorioLog;

                string arquivo =
                     string.Format(
                             "log_{0}_{1}_{2}.txt",
                             DateTime.Now.Year.ToString(),
                             Util.Right(DateTime.Now.Month.ToString("00"), 2),
                             Util.Right(DateTime.Now.Day.ToString("00"), 2)
                         );


                string texto =
                string.Format(
                           "Mensagem.........: {0}\r\n" +
                           "Data/Hora........: {1}\r\n" +
                           "Stored Procedure.: {2}\r\n" +
                           "Usuário..........: {3}\r\n" +
                           "Dados............: \r\n{4}\r\n" +
                           "Exception........: \r\n{5}\r\n" +
                           ".............................................................................\r\n",
                           e.Message,
                           DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                           proc ?? "Não identificada.",
                           usuario ?? "Não identificado.",
                           JsonConvert.SerializeObject(dados ?? "Não identificado", Formatting.Indented),
                           JsonConvert.SerializeObject(e, Formatting.Indented)
                       );

                Arquivo.CriarEditaAquivoTexto(path, arquivo, texto);

            }
            catch (Exception ex)
            {
                string mensagem = string.Format(
                            "Olá desenvolvedor<br />" +
                            "Ocorreu um erro no método ArquiteturaBase.COMM.Service.ArquivoService.ArquivoLogErro<br />" +
                            "<b>Exception:</b><br /> <pre>{0}</pre>",
                            JsonConvert.SerializeObject(ex, Formatting.Indented)
                    );
                EmailService.EmailAlertaDesenvolvedor(mensagem);
            }
        }
    }
}
