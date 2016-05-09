using System.Net.Http;
using System.Web.Http;

namespace Hermes.WebApi.Test.ErrorLog
{
	[AllowAnonymous]
	public class ErrorResultController : ApiController
	{
		public HttpResponseMessage Get()
		{
			string result = null;

			// Object reference exception
			return Request.CreateResponse(result.ToString());
		}
	}
}