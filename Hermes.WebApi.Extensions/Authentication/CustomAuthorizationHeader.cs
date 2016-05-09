using Hermes.WebApi.Core.Common;
using Hermes.WebApi.Core.Interfaces;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace Hermes.WebApi.Extensions.Authentication
{
	public class CustomAuthorizationHeader : SkipAuthorizationBase, IAuthentication
	{
		public string ErrorMessage { get; set; }

		public async Task<IPrincipal> AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			var request = context.Request;
			var authenticationHeader = request.Headers.Authorization;

			if (authenticationHeader == null
				|| authenticationHeader.Scheme != "GTZ"
				|| String.IsNullOrEmpty(authenticationHeader.Parameter))
			{
				return null;
			}

			string headerValue = GetHeaderValue(request);

			IPrincipal principal;

			principal = await Task.Run(() =>
			{
				return AuthenticateUsernamePassword(headerValue);
			});

			return principal;
		}

		public IPrincipal AuthenticateUsernamePassword(string headerValue)
		{
			throw new NotImplementedException();
		}

		private string GetHeaderValue(System.Net.Http.HttpRequestMessage request)
		{
			throw new NotImplementedException();
		}

		public IPrincipal Authenticate(HttpRequestBase httpRequestBase)
		{
			throw new NotImplementedException();
		}
	}
}