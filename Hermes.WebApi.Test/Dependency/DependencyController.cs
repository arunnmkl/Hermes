using GlobalTranz.WebApi.Helper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GlobalTranz.WebApi.Test.Dependency
{
	public class DependencyController : ApiControllerBase
	{
		private IAuthentication _authentication;

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyController"/> class.
		/// </summary>
		/// <param name="authentication">The authentication.</param>
		public DependencyController(IAuthentication authentication = null)
		{
			_authentication = authentication;
		}

		public IHttpActionResult Get()
		{
			return Ok();
		}
	}
}
