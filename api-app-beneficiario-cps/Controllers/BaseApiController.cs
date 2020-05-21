using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace api_app_beneficiario_cps.Controllers
{
    public class BaseApiController : ApiController, IDisposable
    {
        public string _stp;
        public string _cnx;
		public int _id = 0;

		public BaseApiController()
		{
			var identity = User.Identity as ClaimsIdentity;
			var claim = identity.Claims.FirstOrDefault(c => c.Type == "id");
			
			this._id = claim == null ? 0 : Convert.ToInt32(claim.Value);
		}

        [ApiExplorerSettings(IgnoreApi = true)]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}