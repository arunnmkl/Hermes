using Hermes.WebApi.Core.Enums;
using Hermes.WebApi.Core.Extensions;
using System;
using System.Web.Http;

namespace Hermes.WebApi.Test.CachedResult
{
	public class CachedResultController : ApiControllerBase
	{
		public IHttpActionResult GetExample(string name)
		{
			return Ok("Hello, " + name).Cached(Cacheability.Public, maxAge: TimeSpan.FromMinutes(15));
		}
	}
}