using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using api_app_beneficiario_cps.App_Start;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace api_app_beneficiario_cps
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // --------------------------------------- DEAFULT --------------------------------
            //// Web API routes
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            // Web API configuration and services

            //-------------------------------------- CUSTOM 1 ---------------------------------
            //var cors = new EnableCorsAttribute("*", "*", "GET, POST, PUT, DELETE, OPTIONS");
            //config.EnableCors(cors);

            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.None;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            // jsonSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            jsonSettings.Culture = CultureInfo.GetCultureInfo("pt-br");

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new DeflateCompressionAttribute());
            config.MessageHandlers.Add(new Controllers.AutorizeController());

            config.Routes.MapHttpRoute(
                         name: "DefaultApi",
                         routeTemplate: "api/{controller}/{action}/{id}",
                         defaults: new { id = RouteParameter.Optional }
                     );

            //---------------------------------- CUSTOM 2 -------------------------------
            //var cors = new EnableCorsAttribute("*", "*", "GET, POST, PUT, DELETE, OPTIONS");
            //config.EnableCors(cors);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);		}
        }
    }
}
