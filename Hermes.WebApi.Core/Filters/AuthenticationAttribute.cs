﻿// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-22-2016
// ***********************************************************************
// <copyright file="AuthenticationAttribute.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary>This class is responsible for all kind of authentication which is registered in the IAuthenticationCommand
// This goes in the same order which is registered</summary>
// ***********************************************************************
using Hermes.WebApi.Core.Results;
using Hermes.WebApi.Core.Security;
using Hermes.WebApi.Core.Interfaces;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Hermes.WebApi.Core.Filters
{
	/// <summary>
	/// This class is responsible for all kind of authentication which is registered in the IAuthenticationCommand.
	/// </summary>
	public class AuthenticationAttribute : Attribute, IAuthenticationFilter
	{
		#region Properties

		/// <summary>
		/// Gets or sets the realm.
		/// </summary>
		/// <value>The realm.</value>
		public string Realm { get; set; }

		/// <summary>
		/// Gets the authentication command.
		/// </summary>
		/// <value>The authentication command.</value>
		public IAuthenticationCommand AuthenticationCommand { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.
		/// </summary>
		/// <value><c>true</c> if [allow multiple]; otherwise, <c>false</c>.</value>
		public bool AllowMultiple
		{
			get { return false; }
		}

		#endregion Properties

		#region AuthenticationAttribute

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationAttribute" /> class.
		/// </summary>
		public AuthenticationAttribute()
		{
			AuthenticationCommand = DependencyResolverContainer.Resolve<IAuthenticationCommand>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationAttribute" /> class.
		/// </summary>
		/// <param name="authenticationCommand">The authentication command.</param>
		public AuthenticationAttribute(IAuthenticationCommand authenticationCommand)
		{
			AuthenticationCommand = authenticationCommand;
		}

		#endregion AuthenticationAttribute

		#region IAuthenticationFilter

		/// <summary>
		/// Authenticates the request.
		/// </summary>
		/// <param name="context">The authentication context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>A Task that will perform authentication.</returns>
		public async Task AuthenticateAsync(HttpAuthenticationContext context, System.Threading.CancellationToken cancellationToken)
		{
			if (AuthenticationCommand != null)
			{
                bool skipAuth = false;
				if (AuthenticationCommand.SkipAuthorization(context.ActionContext))
				{
                    skipAuth = true;
                }

				IPrincipal principal = null;
				string message = "Invalid username or password";

				foreach (var item in AuthenticationCommand.AuthenticationCommands)
				{
					principal = await item.AuthenticateAsync(context, cancellationToken);

					if (principal != null)
					{
						if (!IsBasicAuthentication(item) && Configuration.Current.CSRFAttackPrevented 
							&& AuthenticationCommand.ValidateCSRFAttack(context))
						{
							context.ErrorResult = new AuthenticationFailureResult("Invalid token", context.Request);
							return;
						}

						SetPrincipal(context, principal);
						return;
					}

					message = item.ErrorMessage ?? message;
				}

				// Authentication was attempted but failed. Set ErrorResult to indicate an error.
				// Authentication was attempted and succeeded. Set Principal to the authenticated user.
                // Do not add challanges for the controllers which are passed for do not authenticate
				if (principal == null && !skipAuth)
				{
					context.ErrorResult = context.ErrorResult ?? new AuthenticationFailureResult(message, context.Request);
				}
			}
		}

		/// <summary>
		/// Adds the Challenges asynchronous, which is used for the handshaking to the browsers.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Task.</returns>
		public Task ChallengeAsync(HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
		{
			Challenge(context);
			return Task.FromResult(0);
		}

		#endregion IAuthenticationFilter

		#region Private Methods

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
		

		/// <summary>
		/// Determines whether [is basic authentication] [the specified HTTP request message].
		/// </summary>
		/// <param name="httpRequestMessage">The HTTP request message.</param>
		/// <returns>True is yes, false otherwise</returns>
		private bool IsBasicAuthentication(System.Net.Http.HttpRequestMessage httpRequestMessage)
		{
			var authenticationHeader = httpRequestMessage.Headers.Authorization;

			if (authenticationHeader == null
				|| authenticationHeader.Scheme != "Basic"
				|| String.IsNullOrEmpty(authenticationHeader.Parameter))
			{
				// No authentication was attempted (for this authentication method).
				// Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
				return false;
			}

			return true;
		}

		/// <summary>
		/// Determines whether [is basic authentication] [the specified item].
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>True is yes, false otherwise</returns>
		private bool IsBasicAuthentication(IAuthentication item)
		{
			return item is IBasicAuthentication;
		}

		#endregion Private Methods
	}
}