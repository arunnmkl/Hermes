using GlobalTranz.WebApi.Helper.Interfaces;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace GlobalTranz.WebApi.Core.Filters
{
	public class CookieAuthenticationFilter : IAuthenticationFilter
	{
		/// <summary>
		/// The cookie authentication
		/// </summary>
		private ICookieAuthentication _cookieAuthentication;

		/// <summary>
		/// Gets or sets the realm.
		/// </summary>
		/// <value>
		/// The realm.
		/// </value>
		public string Realm { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.
		/// </summary>
		public bool AllowMultiple
		{
			get { return false; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CookieAuthenticationFilter"/> class.
		/// </summary>
		public CookieAuthenticationFilter()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CookieAuthenticationFilter"/> class.
		/// </summary>
		/// <param name="cookieAuthentication">The cookie authentication.</param>
		public CookieAuthenticationFilter(ICookieAuthentication cookieAuthentication)
		{
			this._cookieAuthentication = cookieAuthentication ?? DependencyResolverContainer.Resolve<ICookieAuthentication>();
		}

		/// <summary>
		/// Authenticates the request.
		/// </summary>
		/// <param name="context">The authentication context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>
		/// A Task that will perform authentication.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public async Task AuthenticateAsync(HttpAuthenticationContext context, System.Threading.CancellationToken cancellationToken)
		{
			// Already authenticated, Permits working with other authentication systems, such as Forms Auth
			if (context.Principal != null && context.Principal.Identity.IsAuthenticated)
			{
				return;
			}

			var request = context.Request;
			var authCookie = request.GetCookie("CookieName");
			var authToken = request.GetQueryString("CookieName");

			IPrincipal principal = await AuthenticateAsync(authCookie, authToken, cancellationToken);

			SetPrincipal(context, principal);
		}

		/// <summary>
		/// Challenges the asynchronous.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			Challenge(context);
			return Task.FromResult(0);
		}

		/// <summary>
		/// Authenticates the asynchronous.
		/// </summary>
		/// <param name="authCookie">The authentication cookie.</param>
		/// <param name="authToken">The authentication token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task<IPrincipal> AuthenticateAsync(string authCookie, string authToken, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var pricipal = await Task.Run(() => { return _cookieAuthentication.AuthenticateCookie(authCookie, authToken); });

			cancellationToken.ThrowIfCancellationRequested();

			return pricipal;
		}

		/// <summary>
		/// Challenges the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Challenge(HttpAuthenticationChallengeContext context)
		{
			string parameter;

			if (String.IsNullOrEmpty(Realm))
			{
				parameter = null;
			}
			else
			{
				// A correct implementation should verify that Realm does not contain a quote character unless properly
				// escaped (proceeded by a backslash that is not itself escaped).
				parameter = "realm=\"" + Realm + "\"";
			}

			context.ChallengeWith("Basic", parameter);
		}

		/// <summary>
		/// Sets the principal.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="principal">The principal.</param>
		private void SetPrincipal(HttpAuthenticationContext context, IPrincipal principal)
		{
			Thread.CurrentPrincipal = principal;
			if (context != null)
			{
				context.Principal = principal;
			}
		}
	}
}