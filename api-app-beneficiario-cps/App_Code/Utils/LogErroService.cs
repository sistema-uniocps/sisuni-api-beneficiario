using System;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    /// <summary>
    /// Cria log de erros
    /// </summary>
    public static class LogErroService
    {
        /// <summary>
        /// Responsavel por criar os logs de erro
        /// </summary>
        /// <param name="logErroTexto">Cria o log de erro em arquivo texto [Diretorio ]</param>
        /// <param name="logErroEmail">Envia o log de erro por e-mail</param>
        /// <param name="e">Exception que foi gerada</param>
        /// <param name="usuario">Usuário logado [opcional]</param>
        /// <param name="dados">Dados [opcional]</param>
        public static void CriaLog(
           bool logErroTexto,
           bool logErroEmail,
           Exception e,
           string proc = null,
           string usuario = null,
           object dados = null)
        {
            #region Salva Log de erro caso seja necessário

            if (logErroTexto && AppSetting.IsSalvaArquivoLog)
            {
                ArquivoService.ArquivoLogErro(e, proc, usuario, dados);
            }

            if (logErroEmail && AppSetting.IsEnviaEmailLog)
            {
                EmailService.EmailLogErro(e, proc, usuario, dados, proc);
            }
            

            #endregion
        }
    }
}
