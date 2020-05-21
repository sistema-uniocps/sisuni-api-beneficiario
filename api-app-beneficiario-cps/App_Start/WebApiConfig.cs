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
			// Web API configuration and services
			var formatters = config.Formatters;
			formatters.Remove(formatters.XmlFormatter);

			var jsonSettings = formatters.JsonFormatter.SerializerSettings;
			jsonSettings.Formatting = Formatting.None;
			jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			jsonSettings.NullValueHandling = NullValueHandling.Ignore;
			jsonSettings.Culture = CultureInfo.GetCultureInfo("pt-br");

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Filters.Add(new DeflateCompressionAttribute());
			//config.MessageHandlers.Add(new Controllers.AutorizeGeralAttribute());

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
