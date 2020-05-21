using System;


namespace api_app_beneficiario_cps.App_Code.Utils
{
    /// <summary>
    /// Recupera todas as keys do  appSettings na web.config
    /// </summary>
    public static class AppSetting
    { 
        #region Configurações de segurança da API

        public static string http_user_login { get { return GetAppSettings<string>("Api.http_user_login"); } }
        public static string http_user_password { get { return GetAppSettings<string>("Api.http_user_password"); } }

        #endregion

        #region Configurações do Sistema

        public static string NomeProjeto { get { return GetAppSettings<string>("Sistema.NomeProjeto"); } }
        public static bool isDebug { get { return GetAppSettings<bool>("Sistema.isDebug"); } }

        #endregion

        #region Configurações do Log de Erro

        public static string EmailRecebeLogErro { get { return GetAppSettings<string>("Erro.EmailRecebeLogErro"); } }
        public static bool IsEnviaEmailLog { get { return GetAppSettings<bool>("Erro.isEnviaEmailLog"); } }
        public static bool IsSalvaArquivoLog { get { return GetAppSettings<bool>("Erro.isSalvaArquivoLog"); } }
        public static string DiretorioLog { get { return GetAppSettings<string>("Erro.DiretorioLog"); } }

        #endregion

        #region Mensagem de Alerta ao Desenvolvedor

        public static string EmailAlertaDesenvolvedor { get { return GetAppSettings<string>("Erro.EmailRecebeLogErro"); } }

        #endregion

        #region Configurações de Email

        public static string Host { get { return GetAppSettings<string>("Email.Host"); } }
        public static int Port { get { return GetAppSettings<int>("Email.Port"); } }
        public static bool EnableSsl { get { return GetAppSettings<bool>("Email.EnableSsl"); } }
        public static bool UseDefaultCredentials { get { return GetAppSettings<bool>("Email.UseDefaultCredentials"); } }
        public static string UserName { get { return GetAppSettings<string>("Email.UserName"); } }
        public static string Password { get { return GetAppSettings<string>("Email.Password"); } }
        public static string TempleteEmail { get { return GetAppSettings<string>("Email.TampleteEmail"); } }

        #endregion

        #region Configuração de Notificação

        public static bool isEnviaNotificacaoSMS { get { return GetAppSettings<bool>("Notificacao.isEnviaNotificacaoSMS"); } }
        public static bool isEnviaNotificacaoEmail { get { return GetAppSettings<bool>("Notificacao.isEnviaNotificacaoEmail"); } }

        #endregion

        #region Privado

        /// <summary>
        /// Método que faz a obtenção da key da web.config
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <return s></return s>
        private static T GetAppSettings<T>(string key)
        {
            return (T)Convert.ChangeType(
                    System.Configuration.ConfigurationManager.AppSettings[key],
                    typeof(T)
                );
        }

        #endregion
    }
}
