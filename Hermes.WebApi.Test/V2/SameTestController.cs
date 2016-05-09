using System.Web.Http;
using Hermes.WebApi.Core;

namespace Hermes.V2
{
	public class SameTestController : ApiController
	{
		public IHttpActionResult Get()
		{
			return new HttpActionResultBase(this.Request, string.Empty);
		}
	}
}