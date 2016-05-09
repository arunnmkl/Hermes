using Hermes.WebApi.Core.Filters;
using System.Web.Http;

namespace Hermes.Authentication
{
	public class AuthenticationController : ApiController
	{
		public bool Get()
		{
			return true;
		}
	}

	[Authorize(Roles = "UnitTest Group")]
	public class RoleAuthorizationController : ApiController
	{
		public bool Get()
		{
			return true;
		}
	}

	[AllowAnonymous]
	public class AnonymousAuthorizationController : ApiController
	{
		[AllowAnonymous]
		public bool Get()
		{
			return true;
		}
	}

	[HermesAuthorization("Access This", "Read")]
	public class SimplexAuthorizationController : ApiController
	{
		public bool Get()
		{
			return true;
		}
	}
}