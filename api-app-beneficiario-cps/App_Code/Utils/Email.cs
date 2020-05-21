using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    /// <summary>
    /// Dados de configuração do serviço Smtp
    /// </summary>
    public class DadosSmtp
    {
        /// <summary>
        /// Ex: smtp.gmail.com
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// Ex: 587
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Ex: true
        /// </summary>
        public bool EnableSsl { get; private set; }

        /// <summary>
        /// Ex: email@gmail.com
        /// </summary>
        public bool UseDefaultCredentials { get; private set; }

        /// <summary>
        /// Ex: email@gmail.com
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Ex: 123456
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Construtor da Class DadosSmtp
        /// </summary>
        /// <param name="Host">Ex: smtp.gmail.com </param>
        /// <param name="Port">Ex: 587</param>
        /// <param name="EnableSsl">Ex: true</param>
        /// <param name="UserName">Ex: email@gmail.com</param>
        /// <param name="Password">Ex: 123456</param>
        public DadosSmtp(string Host, int Port, bool EnableSsl, string UserName, string Password)
        {
            this.Host = Host;
            this.Port = Port;
            this.EnableSsl = EnableSsl;
            this.UseDefaultCredentials = false;
            this.UserName = UserName;
            this.Password = Password;
        }
        /// <summary>
        /// Busca dados no appSettings da web.config
        /// </summary>
        public DadosSmtp()
        {
            this.Host = AppSetting.Host;
            this.Port = AppSetting.Port;
            this.EnableSsl = AppSetting.EnableSsl;
            this.UseDefaultCredentials = AppSetting.UseDefaultCredentials;
            this.UserName = AppSetting.UserName;
            this.Password = AppSetting.Password;
        }

    }

    /// <summary>
    /// Dados que serão usados para o envio de e-mail
    /// </summary>
    public class DadosEmail
    {
        /// <summary>
        /// Esse é o e-mail que receberá a mensagem. Pode ser passado mais de um separando por ";
        /// </summary>
        public string EmailDestino { get; set; }

        /// <summary>
        /// Assunto que será exibido na caixa de entrada
        /// </summary>
        public string Assunto { get; set; }

        /// <summary>
        /// Mensagem do corpo do e-mail. Você pode usar a função "MontaCorpo" para auxiliar a montagem desse corpo da mensagem
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Construtor sem paramentros. Lembre-se de setar as propriedades
        /// </summary>
        public DadosEmail()
        {
        }

        /// <summary>
        /// class dos dados que serão enviados por e-mail
        /// </summary>
        /// <param name="emailDestino">Esse é o e-mail que receberá a mensagem. Pode ser passado mais de um separando por ";"</param>
        /// <param name="assunto">Assunto que será exibido na caixa de entrada</param>
        /// <param name="mensagem">Mensagem do corpo do e-mail. Você pode usar a função "MontaCorpo" para auxiliar a montagem desse corpo da mensagem</param>
        public DadosEmail(string EmailDestino, string Assunto, string Mensagem)
        {
            this.EmailDestino = EmailDestino + ";";
            this.Assunto = Assunto;
            this.Mensagem = Mensagem;
        }

        /// <summary>
        /// Monta o corpo em forma de HTML.
        /// </summary>
        /// <param name="titulo">É o título do corpo da mensagem. Caso você queria pode criar um arquivo HTML em "~\Email\email.htm" com as marcações #TITULO, #SUBTITULO, #MENSAGEM e #RODAPE que o método fará o "Replace" para você</param>
        /// <param name="subTitulo">Irá aparecer logo abaixo do título</param>
        /// <param name="mensgem">É a mensagem em sí</param>
        /// <param name="rodape">É a últma informação que irá aparecer no e-mail</param>
        /// <returns>Retorna a sua mensagem formatada. Caso você queria pode criar um arquivo HTML em "~\Email\email.htm" com as marcações #TITULO, #SUBTITULO, #MENSAGEM e #RODAPE que o método fará o "Replace" para você</returns>
        public string MontaCorpo(string titulo, string subTitulo, string mensgem, string rodape)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~");
            string arquivo = path + AppSetting.TempleteEmail;

            if (!File.Exists(arquivo))
            {
                return string.Format("<b>{0}</b><br />{1}<br /><br />{2}<br /><br />{3}", titulo, subTitulo, mensgem, rodape);
            }
            else
            {
                return File.ReadAllText(arquivo)
                .Replace("{TITULO}", titulo)
                .Replace("{SUBTITULO}", subTitulo)
                .Replace("{MENSAGEM}", mensgem)
                .Replace("{RODAPE}", rodape)
                .Replace("<hr />", "<hr style=\"border: 0; height: 0; border-top: 1px solid #cccccc; border-bottom-width: 0; margin:20px 0\"/>");
            }
        }
    }

    /// <summary>
    /// Classe que faz o envio de e-mails
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Dados de configuração do serviço Smtp
        /// </summary>
        private DadosSmtp DadosSmtp { get; set; }


        /// <summary>
        /// Busca dados SMTP no appSettings da web.config
        /// </summary>
        public Email()
        {
            this.DadosSmtp = new DadosSmtp();
        }

        /// <summary>
        /// É obrigatória a passagem de um objeto da class  DadosSmtp
        /// </summary>
        /// <param name="smtp">esse é um objeto da class DadosSmtp</param>
        public Email(ref DadosSmtp dadosSmtp)
        {
            this.DadosSmtp = dadosSmtp;
        }

        /// <summary>
        /// Método que faz o envio do e-mail
        /// </summary>
        /// <param name="dadosEmail">Dados do e-mail a ser enviado</param>
        /// <param name="assincrona">"true" se quiser o método assincrono e "false" para enviar sincrono</param>
        /// <returns>Retorna a situação do envio</returns>
        public bool Enviar(DadosEmail dadosEmail, bool assincrona = true)
        {
            if (assincrona)
            {
                Task t = Task.Factory.StartNew(() =>
                {
                    Envio(dadosEmail);
                });
                return true;
            }
            else
            {
                return Envio(dadosEmail);
            }
            
        }

        /// <summary>
        /// Método que faz o envio do e-mail
        /// </summary>
        /// <param name="emailDestino">Esse é o e-mail que receberá a mensagem. Pode ser passado mais de um separando por ";"</param>
        /// <param name="assunto">Assunto que será exibido na caixa de entrada</param>
        /// <param name="mensagem">Mensagem do corpo do e-mail. Você pode usar a função "MontaCorpo" para auxiliar a montagem desse corpo da mensagem</param>
        /// <param name="assincrona">"true" se quiser o método assincrono e "false" para enviar sincrono</param>
        /// <returns>Retorna a situação do envio</returns>
        public bool Enviar(string emailDestino, string assunto, string mensagem, bool assincrona = true)
        {
            DadosEmail dadosEmail = new DadosEmail()
            {
                EmailDestino = emailDestino + ";",
                Assunto = assunto,
                Mensagem = mensagem
            };
            return Enviar(dadosEmail, assincrona);
        }

        #region Métodos Privádos

        private bool Envio(DadosEmail dadosEmail)
        {
            bool result = false;
            try
            {
                Valida(dadosEmail);

                string[] mailToVetor = (dadosEmail.EmailDestino.Split(';')).Distinct().ToArray();
                for (int i = 0; i < mailToVetor.Length; i++)
                {
                    if (mailToVetor[i] != "")
                    {
                        MailMessage message = new MailMessage(this.DadosSmtp.UserName, mailToVetor[i], dadosEmail.Assunto, dadosEmail.Mensagem)
                        {
                            IsBodyHtml = true,
                            BodyEncoding = System.Text.Encoding.UTF8
                        };
                        SmtpClient client = new SmtpClient()
                        {
                            Host = this.DadosSmtp.Host,
                            Port = this.DadosSmtp.Port,
                            EnableSsl = this.DadosSmtp.EnableSsl,
                            UseDefaultCredentials = this.DadosSmtp.UseDefaultCredentials
                        };
                        NetworkCredential cred = new NetworkCredential(this.DadosSmtp.UserName, this.DadosSmtp.Password);
                        client.Credentials = cred;
                        client.Send(message);

                        message = null;
                        client.Dispose();
                        client = null;
                    }
                    result = true;
                }
            }
            catch (EnvioEmailException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EnvioEmailException(ex.Message, ex);
            }


            return result;
        }

        private void Valida(DadosEmail dadosEmail)
        {
            if (string.IsNullOrEmpty(dadosEmail.EmailDestino) || !dadosEmail.EmailDestino.Contains("@") || (dadosEmail.EmailDestino.Split(';')).Length == 0)
                throw new EnvioEmailException("E-mail inválido.");

            if (string.IsNullOrEmpty(dadosEmail.Assunto))
                throw new EnvioEmailException("O assunto do e-mail não pode ser vazio.");

            if (string.IsNullOrEmpty(dadosEmail.Mensagem))
                throw new EnvioEmailException("A mensagem não pode ser vazia.");
        }

        #endregion
    }
}
