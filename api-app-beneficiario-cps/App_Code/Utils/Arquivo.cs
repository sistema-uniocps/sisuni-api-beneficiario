using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    public class Arquivo
    {
        public static void CriarEditaAquivoTexto(string path, string nomeArquivo, string texto, bool assincrona = true)
        {
            if (assincrona)
            { 
                Task t = Task.Factory.StartNew(() =>
                {
                    CriarEditaAquivoTexto(path, nomeArquivo, texto);
                });
            }
            else
            {
                CriarEditaAquivoTexto(path, nomeArquivo, texto);
            }
        }

        #region Métodos Privádos

        private static void CriarEditaAquivoTexto(string path, string nomeArquivo, string texto)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            StreamWriter vWriter = new StreamWriter(path + nomeArquivo, true);
            vWriter.WriteLine(texto);
            vWriter.WriteLine();
            vWriter.Flush();
            vWriter.Close();
        }
         
        #endregion
    }
}
