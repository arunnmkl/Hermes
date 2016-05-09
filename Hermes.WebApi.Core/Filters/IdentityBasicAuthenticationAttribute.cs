using GlobalTranz.WebApi.Core.Results;
using GlobalTranz.WebApi.Helper.Interfaces;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalTranz.WebApi.Core.Filters
{
	public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityBasicAuthenticationAttribute"/> class.
		/// </summary>
		public IdentityBasicAuthenticationAttribute()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityBasicAuthenticationAttribute"/> class.
		/// </summary>
		/// <param name="authentication">The authentication.</param>
		public IdentityBasicAuthenticationAttribute(IAuthentication authentication)
			: base(authentication)
		{
		}

		/// <summary>
		/// Authenticates the asynchronous.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
		{
			// Cancel the request if support CancellationTokens.
			cancellationToken.ThrowIfCancellationRequested();

			var principal = await Task.Run<IPrincipal>(() =>
			{
				try
				{
					return Authentication.AuthenticateUsernamePassword(userName, password);
				}
				catch (System.Exception ex)
				{
					AuthenticationContext.ErrorResult = new AuthenticationFailureResult(ex.Message, Request);
					return null;
				}
			});

			// Cancel the request if support CancellationTokens.
			cancellationToken.ThrowIfCancellationRequested();

			// If authentication is failed from the Authentication, then send the same error to the client
			if (principal == null && AuthenticationContext.ErrorResult == null)
				AuthenticationContext.ErrorResult = new AuthenticationFailureResult(Authentication.ErrorMessage, Request);

			return principal;
		}
	}
}