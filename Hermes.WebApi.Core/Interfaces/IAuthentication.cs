// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-22-2016
// ***********************************************************************
// <copyright file="IAuthentication.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Hermes.WebApi.Core.Interfaces
{
	/// <summary>
	/// Interface IAuthentication
	/// </summary>
	public interface IAuthentication
	{
		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		/// <value>The error message.</value>
		string ErrorMessage { get; set; }

		/// <summary>
		/// Skips the authorization.
		/// </summary>
		/// <param name="actionContext">The action context.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool SkipAuthorization(HttpActionContext actionContext);

		/// <summary>
		/// Authenticates the asynchronous.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Task&lt;IPrincipal&gt;.</returns>
		Task<IPrincipal> AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken);

		/// <summary>
		/// Authenticates the specified HTTP request base.
		/// </summary>
		/// <param name="httpRequestBase">The HTTP request base.</param>
		/// <returns>IPrincipal.</returns>
		IPrincipal Authenticate(HttpRequestBase httpRequestBase);
	}
}