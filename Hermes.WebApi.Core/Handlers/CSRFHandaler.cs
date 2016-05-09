// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="CSRFHandaler.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using Hermes.WebApi.Core.Security;
using Hermes.WebApi.Core.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes.WebApi.Core.Handlers
{
	/// <summary>
	/// A handler which handles the CSRF attack and adds the values to the cookie
	/// </summary>
	public class CSRFHandler : DelegatingHandler
	{
		/// <summary>
		/// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
		/// </summary>
		/// <param name="request">The HTTP request message to send to the server.</param>
		/// <param name="cancellationToken">A cancellation token to cancel operation.</param>
		/// <returns>Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.</returns>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var response = await base.SendAsync(request, cancellationToken);

			// throw the exception if requested, cancel the the request
			cancellationToken.ThrowIfCancellationRequested();

			if (response.IsSuccessStatusCode && Configuration.Current.CSRFAttackPrevented)
			{
				ICSRFValidation validator = DependencyResolverContainer.Resolve<ICSRFValidation>();
				if (validator != null)
				{
					string csrfNumber = validator.GetCSRFValue();
					CookieHeaderValue csrfCookie = new CookieHeaderValue(Configuration.Current.CSRFCookieName, csrfNumber);
					csrfCookie.Path = "/";
					response.Headers.AddCookies(new CookieHeaderValue[] { csrfCookie });
				}
			}

			return response;
		}
	}
}