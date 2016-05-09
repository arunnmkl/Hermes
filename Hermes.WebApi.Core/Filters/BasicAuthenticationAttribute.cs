using GlobalTranz.WebApi.Core.Results;
using GlobalTranz.WebApi.Core.Security;
using GlobalTranz.WebApi.Helper.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace GlobalTranz.WebApi.Core.Filters
{
	public sealed class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
	{
		#region Properties

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
		/// Gets the authentication header.
		/// </summary>
		/// <value>
		/// The authentication header.
		/// </value>
		public AuthenticationHeaderValue AuthenticationHeader { get; private set; }

		/// <summary>
		/// Gets or sets the authentication.
		/// </summary>
		/// <value>
		/// The authentication.
		/// </value>
		public IBasicAuthentication Authentication { get; private set; }

		/// <summary>
		/// Gets the authentication context.
		/// </summary>
		/// <value>
		/// The authentication context.
		/// </value>
		public HttpAuthenticationContext AuthenticationContext { get; private set; }

		/// <summary>
		/// Gets the request.
		/// </summary>
		/// <value>
		/// The request.
		/// </value>
		public HttpRequestMessage Request { get; private set; }

		#endregion Properties

		#region BasicAuthenticationAttribute

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicAuthenticationAttribute"/> class.
		/// </summary>
		public BasicAuthenticationAttribute()
		{
			Authentication = DependencyResolverContainer.Resolve<IBasicAuthentication>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicAuthenticationAttribute"/> class.
		/// </summary>
		/// <param name="authentication">The authentication.</param>
		public BasicAuthenticationAttribute(IBasicAuthentication authentication)
		{
			Authentication = authentication ?? DependencyResolverContainer.Resolve<IBasicAuthentication>();
		}

		#endregion BasicAuthenticationAttribute

		#region Public Methods

		/// <summary>
		/// Authenticates the request.
		/// </summary>
		/// <param name="context">The authentication context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>
		/// A Task that will perform authentication.
		/// </returns>
		public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			if (Authentication.SkipAuthorization(context.ActionContext))
			{
				return;
			}

			IPrincipal principal = null;
			if (Configuration.Current.BasicAuthenticationEnabled)
			{
				principal = await Basic(context, cancellationToken);
			}


			// Authentication was attempted but failed. Set ErrorResult to indicate an error.
			// Authentication was attempted and succeeded. Set Principal to the authenticated user.
			if (principal == null)
			{
				context.ErrorResult = context.ErrorResult ?? new AuthenticationFailureResult("Invalid username or password", Request);
			}
			else
			{
				SetPrincipal(context, principal);
			}
		}

		private async Task<IPrincipal> Basic(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			AuthenticationContext = context;
			Request = context.Request;
			AuthenticationHeader = Request.Headers.Authorization;

			if (AuthenticationHeader == null
				|| AuthenticationHeader.Scheme != "Basic"
				|| String.IsNullOrEmpty(AuthenticationHeader.Parameter))
			{
				// No authentication was attempted (for this authentication method).
				// Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
				context.ErrorResult = new AuthenticationFailureResult("Missing credentials", Request);
				//return;
			}

			Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(AuthenticationHeader.Parameter);

			if (userNameAndPasword == null)
			{
				// Authentication was attempted but failed. Set ErrorResult to indicate an error.
				context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", Request);
				//return;
			}

			var userName = userNameAndPasword.Item1;
			var password = userNameAndPasword.Item2;
			IPrincipal principal = await AuthenticateAsync(userName, password, cancellationToken);

			return principal;
		}

		/// <summary>
		/// Challenges the asynchronous.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			Challenge(context);
			return Task.FromResult(0);
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Authenticates the asynchronous.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		private async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
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

		#endregion Protected Methods

		#region Private Methods

		/// <summary>
		/// Extracts the user name and password.
		/// </summary>
		/// <param name="authorizationParameter">The authorization parameter.</param>
		/// <returns></returns>
		private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
		{
			byte[] credentialBytes;

			try
			{
				credentialBytes = Convert.FromBase64String(authorizationParameter);
			}
			catch (FormatException)
			{
				return null;
			}

			// The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
			// However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
			// used in practice and defines behavior only for ASCII.
			Encoding encoding = Encoding.ASCII;

			// Make a writable copy of the encoding to enable setting a decoder fallback.
			encoding = (Encoding)encoding.Clone();

			// Fail on invalid bytes rather than silently replacing and continuing.
			encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
			string decodedCredentials;

			try
			{
				decodedCredentials = encoding.GetString(credentialBytes);
			}
			catch (DecoderFallbackException)
			{
				return null;
			}

			if (String.IsNullOrEmpty(decodedCredentials))
			{
				return null;
			}

			int colonIndex = decodedCredentials.IndexOf(':');
			if (colonIndex == -1)
			{
				return null;
			}

			string userName = decodedCredentials.Substring(0, colonIndex);
			string password = decodedCredentials.Substring(colonIndex + 1);
			return new Tuple<string, string>(userName, password);
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

		#endregion Private Methods
	}
}