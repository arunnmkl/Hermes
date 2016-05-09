// ***********************************************************************
// Assembly         : Hermes.WebApi.Extensions
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-22-2016
// ***********************************************************************
// <copyright file="CookieAuthenticationController.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using AmadeusConsulting.Simplex.Security;
using Hermes.WebApi.Core;
using Hermes.WebApi.Core.Common;
using Hermes.WebApi.Core.Interfaces;
using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using HermesSecurity = Hermes.WebApi.Core.Security;

namespace Hermes.WebApi.Extensions.Authentication
{
	/// <summary>
	/// Class class CookieAuthenticationController.
	/// </summary>
	public class CookieAuthenticationController : SkipAuthorizationBase, ICookieAuthentication
	{
		/// <summary>
		/// Gets or sets the error/Exception message.
		/// </summary>
		/// <value>The error message.</value>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// Authenticates the cookie.
		/// </summary>
		/// <param name="ticket">The ticket.</param>
		/// <returns>IPrincipal.</returns>
		public IPrincipal AuthenticateCookie(string ticket)
		{
			IPrincipal principal = null;

			try
			{
				if (!string.IsNullOrEmpty(ticket))
				{
					principal = AuthenticationCommands.AuthenticateTicket(ticket);
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}

			return principal;
		}

		/// <summary>
		/// Authenticates the asynchronous.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Task&lt;IPrincipal&gt;.</returns>
		public async Task<IPrincipal> AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			IPrincipal principal = null;
			HttpRequestMessage request = context.Request;
			string authCookie = request.GetCookie(HermesSecurity.Configuration.Current.AuthCookieName);
			string authToken = request.GetQueryString(HermesSecurity.Configuration.Current.AuthCookieName);

			string ticket = authCookie != null ? authCookie : null;
			if (ticket == null && !string.IsNullOrWhiteSpace(authToken))
			{
				ticket = Uri.UnescapeDataString(authToken);
			}

			if (string.IsNullOrEmpty(ticket))
			{
				ErrorMessage = "Invalid credentials.";
				return principal;
			}

			principal = await Task.Run(() =>
			{
				return AuthenticateCookie(ticket);
			});

			return principal;
		}

		/// <summary>
		/// Authenticates the specified HTTP request base. This is used in the MVC applications.
		/// </summary>
		/// <param name="httpRequestBase">The HTTP request base.</param>
		/// <returns>The implementation of the current principle</returns>
		public IPrincipal Authenticate(System.Web.HttpRequestBase httpRequestBase)
		{
			var value = httpRequestBase.Cookies[HermesSecurity.Configuration.Current.AuthCookieName];

			if (value != null)
			{
				string ticket = ticket = Uri.UnescapeDataString(value.Value);

				var principal = AuthenticateCookie(ticket);

				return principal;
			}

			return null;
		}
	}
}