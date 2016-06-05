using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Hermes.WebApi.Core.Interfaces;
using Hermes.WebApi.Core.Results;

namespace Hermes.WebApi.Core.Common
{
    /// <summary>
    /// Bearer authentication base class.
    /// </summary>
    /// <seealso cref="Hermes.WebApi.Core.Common.SkipAuthorizationBase" />
    /// <seealso cref="Hermes.WebApi.Core.Interfaces.IBearerAuthentication" />
    public abstract class BearerAuthenticationBase : SkipAuthorizationBase, IBearerAuthentication
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public ResponseError ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        string IAuthentication.ErrorMessage
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Authenticates the token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="context"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// true whether the token is valid else return false.
        /// </returns>
        public abstract Task<bool> AuthenticateToken(string accessToken, HttpAuthenticationContext context, CancellationToken cancellationToken);

        /// <summary>
        /// Authenticates the specified HTTP request base.
        /// </summary>
        /// <param name="httpRequestBase">The HTTP request base.</param>
        /// <returns>
        /// IPrincipal.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IPrincipal Authenticate(HttpRequestBase httpRequestBase)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Authenticates the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// Task&lt;IPrincipal&gt;.
        /// </returns>
        public async Task<IPrincipal> AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for token in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no authorization token in header, do nothing.
            if (authorization == null)
            {
                return null;
            }

            // 3. If there are authorization token but the filter does not recognize the 
            //    authentication scheme, do nothing.
            if (authorization.Scheme != "Bearer")
            {
                ErrorMessage = AuthorizeResponseMessage.RequireAuthorization;
                return null;
            }

            // 4. If there are authorization token that the filter understands, try to validate them.
            // 5. If the authorization token are empty/bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                ErrorMessage = AuthorizeResponseMessage.MissingAccessToken;
                return null;
            }

            bool isValid = await Task.Run(() => { return AuthenticateToken(authorization.Parameter, context, cancellationToken); });

            if (isValid)
            {
                return context.Principal;
            }

            return null;
        }
    }
}
