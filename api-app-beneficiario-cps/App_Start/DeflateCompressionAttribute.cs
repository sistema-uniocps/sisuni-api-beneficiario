using api_app_beneficiario_cps.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace api_app_beneficiario_cps.App_Start
{
    public class DeflateCompressionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var content = actionExecutedContext.Response.Content;
            var bytes = content?.ReadAsByteArrayAsync().Result;
            var compressedContent = bytes == null ? new byte[0] :
            CompressionHelper.DeflateByte(bytes);
            actionExecutedContext.Response.Content = new ByteArrayContent(compressedContent);
            actionExecutedContext.Response.Content.Headers.Add("Content-encoding", "deflate");
            actionExecutedContext.Response.Content.Headers.Remove("Content-Type");
            actionExecutedContext.Response.Content.Headers.Add("Content-Type", "application/json");
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}