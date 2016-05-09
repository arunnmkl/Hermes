using AmadeusConsulting.Simplex.Security;
using Hermes.WebApi.Core;
using Hermes.WebApi.Web.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Hermes.WebApi.Web.Controllers
{
	[AllowAnonymous]
	public class LoginController : ApiControllerBase
	{
		[AllowAnonymous]
		public HttpResponseMessage Post(Login login)
		{
			HttpResponseMessage response;
			try
			{
				var principal = AuthenticationCommands.AuthenticateUsernamePassword(login.Username, login.Password);

				response = new HttpResponseMessage(HttpStatusCode.OK);
				response.SetAuthentication(HttpUtility.UrlEncode(principal.Identity.SecureTicketString));
				//response.AddCookie("SimplexOAuth", HttpUtility.UrlEncode(principal.Identity.SecureTicketString));
				//response.Headers.Add("Allow-Access-Control-Credentials", "true");
			}
			catch (Exception)
			{
				response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			}

			response.RequestMessage = Request;
			return response;
		}
	}
}