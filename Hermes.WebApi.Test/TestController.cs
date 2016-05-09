using System.Net;
using System.Web.Http;
using Hermes.WebApi.Core;

namespace Hermes
{
	public class TestController : ApiController
	{
		public IHttpActionResult Get()
		{
			return new HttpActionResultBase(this.Request, string.Empty);
		}

		public IHttpActionResult Get(int number)
		{
			if (number > 0)
			{
				return new HttpActionResultBase(this.Request, number);
			}

			return NotFound();
		}

		public IHttpActionResult GetProducts(int product)
		{
			if (product > 0)
			{
				return new HttpActionResultBase(this.Request, product);
			}

			return NotFound();
		}

		public IHttpActionResult Get(long status)
		{
			if (status > 0)
			{
				return new HttpActionResultBase(this.Request, status);
			}

			return NotFound();
		}

		public IHttpActionResult PutProduct(string productName)
		{
			return new HttpActionResultBase(this.Request, productName);
		}
	}
}