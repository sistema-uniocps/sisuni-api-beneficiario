using System.Web;
using System.Web.Mvc;

namespace api_app_beneficiario_cps
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
