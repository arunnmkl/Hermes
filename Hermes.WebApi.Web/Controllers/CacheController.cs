using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hermes.WebApi.Core.Extensions;
using Hermes.WebApi.Core.Enums;

namespace Hermes.WebApi.Web.Controllers
{
	public class CacheController : ApiController
	{
		public IHttpActionResult Get(int id)
		{
			return Ok(id).Cached(Cacheability.Private, maxAge: TimeSpan.FromMinutes(15));
		}

		public HttpResponseMessage Get()
		{
			return Request.CreateResponse("Hello Cached").CacheResponse(Cacheability.Private, maxAge: TimeSpan.FromMinutes(15));
		}
	}
}
