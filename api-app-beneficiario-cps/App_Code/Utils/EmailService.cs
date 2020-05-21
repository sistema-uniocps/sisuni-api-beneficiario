using Newtonsoft.Json;
using System;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    /// <summary>
    /// Autor.......: Renato de Ávila Pereira
    /// Criada em...: 06/09/2017
    /// Descrição...: Serviço de envio de E-mails
    /// ............. Essa classe usa  [Class Email] [Class DadosEmail] do arquivo Email.cs
    /// </summary>
    public class EmailService
    {
        #region Propriedades

        private static DadosEmail dadosEmail;
        private static Email email;

        #endregion

        /// <summary>
        /// Envia log de erro para o e-mail do desenvolvedor
        /// Esse método é assincrono
        /// </summary>
        /// <param name="e">Exception gerada</param>
        public static void EmailLogErro(Exception e, string proc, string usuario = null, object dados = null, string procedure = null)
        {
            try
            {
                dadosEmail = new DadosEmail();
                email = new Email();

                string Assunto = "Log de Erro";
                dadosEmail.Assunto = "[ERRO] " + Assunto;
                dadosEmail.EmailDestino = AppSetting.EmailRecebeLogErro;
                dadosEmail.Mensagem = dadosEmail.MontaCorpo
                   (
                        AppSetting.NomeProjeto,
                        Assunto,
                        string.Format(
                            "<h3>Origem da mensagem: API</h3><br />" +
                            "<b>Mensagem:</b> {0} <br /><br />" +
                            "<b>Procedure:</b> {1} <br /><br />" +
                            "<b>Usuário:</b> {2} <br /><br />" +
                            "<b>Data/Hora:</b> {3} <br /><br /> " +
                            "<b>Exception:</b><br /> <pre>{4}</pre>" +
                            "<b>Dados:</b><br /> <pre>{5}</pre>",
                        e.Message,
                        procedure ?? "Não identificada.",
                        usuario ?? "Não identificado.",
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        JsonConvert.SerializeObject(e, Formatting.Indented),
                        JsonConvert.SerializeObject(dados ?? "Não identificado", Formatting.Indented)
                        ),
                        AppSetting.NomeProjeto
                    );

                email.Enviar(dadosEmail);
            }
            catch
            {
                //Não terá tratamento para esse erro.
            }
        }

        /// <summary>
        /// Encaminha uma mensagem para o Desenvolvedor
        /// </summary>
        /// <param name="mensagem">Mensagem</param>
        public static void EmailAlertaDesenvolvedor(string mensagem)
        {
            try
            {
                dadosEmail = new DadosEmail();
                email = new Email();

                string Assunto = "Mensagem para o desenvolvedor";
                dadosEmail.Assunto = "[ALERTA] " + Assunto;
                dadosEmail.EmailDestino = AppSetting.EmailAlertaDesenvolvedor;
                dadosEmail.Mensagem = dadosEmail.MontaCorpo
                    (
                        AppSetting.NomeProjeto,
                        Assunto,
                        string.Format(
                            "<h3>Origem da mensagem: API</h3><br />" +
                            "<b>Mensagem:</b> {0} <br /><br />" +
                            "<b>Data/Hora:</b> {1} <br /><br /> ",
                            mensagem,
                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                        ),
                        AppSetting.NomeProjeto
                    );

                email.Enviar(dadosEmail);
            }
            catch
            {
                //Não terá tratamento para esse erro.
            }
        }
    }
}
