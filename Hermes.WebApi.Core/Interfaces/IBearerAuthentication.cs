using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Hermes.WebApi.Core.Common;

namespace Hermes.WebApi.Core.Interfaces
{
    /// <summary>
    /// interface IBearerAuthentication
    /// </summary>
    /// <seealso cref="Hermes.WebApi.Core.Interfaces.IAuthentication" />
    public interface IBearerAuthentication : IAuthentication
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        new ResponseError ErrorMessage { get; set; }

        /// <summary>
        /// Authenticates the token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// true whether the token is valid else return false.
        /// </returns>
        Task<bool> AuthenticateToken(string accessToken, HttpAuthenticationContext context, CancellationToken cancellationToken); 
    }
}
