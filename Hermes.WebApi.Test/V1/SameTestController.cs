using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Hermes.WebApi.Core;

namespace Hermes.V1
{
	public class SameTestController : ApiController
	{
		public IHttpActionResult Get()
		{
			return new HttpActionResultBase(this.Request, string.Empty);
		}
	}
}
