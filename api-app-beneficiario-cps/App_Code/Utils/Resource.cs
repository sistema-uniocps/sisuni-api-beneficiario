using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace api_app_beneficiario_cps.App_Code.Utils
{
    public static class Resource
    {
        /// <summary>
        /// Efetua uma busca nos aquivos de resouce e retorna a mensagem encontrada
        /// </summary>
        /// <param name="resource">Nome</param>
        /// <param name="encontrouResource">Retorna se encontrou o Resource ou não</param>
        /// <returns></returns>
        public static string Get(string resource, ref bool encontrouResource)
        {
            string mensagem = string.Empty;
            switch (resource)
            {
                case "Mensagem.ErroInesperado":
                    mensagem = Mensagem.ErroInesperado;
                    break;

                default:
                    encontrouResource = false;
                    mensagem = Mensagem.ErroInesperado;
                    break;
            }

            return mensagem;
        }

        public static object GetModelResource()
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            object model = new
            {
                Alerta = ToDictionary(typeof(Mensagem), culture) 
            };
            return model;
        }

        public static object GetModelResource(string languageCode)
        {
            object model = new
            {
                Mensagem = ToDictionary(typeof(Mensagem), languageCode) 
            };
            return model;
        }

        #region Privados 

        private static Dictionary<string, string> ToDictionary(Type resource)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            return ToDictionary(resource, culture);
        }

        private static Dictionary<string, string> ToDictionary(Type resource, string languageCode)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(languageCode);
            return ToDictionary(resource, culture);
        }

        private static Dictionary<string, string> ToDictionary(Type resource, CultureInfo culture)
        {
            Dictionary<string, string> dictionary = ResourceToDictionary(resource, culture);
            return dictionary;
        }

        private static Dictionary<string, string> ResourceToDictionary(Type resource, CultureInfo culture)
        {
            ResourceManager rm = new ResourceManager(resource);
            PropertyInfo[] pis = resource.GetProperties(BindingFlags.Public | BindingFlags.Static);
            IEnumerable<KeyValuePair<string, string>> values =
                from pi in pis
                where pi.PropertyType == typeof(string)
                select new KeyValuePair<string, string>(
                    pi.Name,
                    rm.GetString(pi.Name, culture));
            Dictionary<string, string> dictionary = values.ToDictionary(k => k.Key, v => v.Value);

            return dictionary;
        }

        #endregion
    }
}