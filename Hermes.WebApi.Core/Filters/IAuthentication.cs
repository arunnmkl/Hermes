using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;

namespace GlobalTranz.WebApi.Core.Filters
{
	public interface IAuthentication
	{
		string ErrorMessage { get; set; }

		IPrincipal AuthenticateUsernamePassword(string userName, string password);

		bool SkipAuthorization(HttpActionContext actionContext);

		bool IsAuthorized(HttpActionContext actionContext, string logicalResourceName = null, string permission = null, string action = null);

		HttpResponseMessage GetUnauthorizedResponse(HttpActionContext actionContext);
	}
}