using System;
using System.Threading.Tasks;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    /// <summary>
    /// Autor.......: Renato de Ávila Pereira
    /// Criada em...: 03/09/2017
    /// Descrição...: Gerar uma Task do Tipo T
    /// </summary>
    /// <typeparam name="T">Tipo</typeparam>
    public static class GerarTask<T>
    {
        /// <summary>
        /// Gerar uma Task do Tipo T
        /// </summary>
        /// <param name="result">Tipo T para geração da Task</param>
        /// <returns>Retorna uma Task do Tipo T</returns>
        public static Task<T> Source(T result)
        {
            var tsc = new TaskCompletionSource<T>();
            tsc.SetResult(result);
            return tsc.Task;
        }
    }
}
