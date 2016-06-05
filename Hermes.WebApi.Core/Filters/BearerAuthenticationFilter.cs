using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Hermes.WebApi.Core.Common;
using Hermes.WebApi.Core.Interfaces;
using Hermes.WebApi.Core.Results;
using Hermes.WebApi.Core.Security;

namespace Hermes.WebApi.Core.Filters
{
    /// <summary>
    /// Bearer authentication filter
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    /// <seealso cref="System.Web.Http.Filters.IAuthenticationFilter" />
    public class BearerAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        /// <summary>
        /// Gets the authentication command.
        /// </summary>
        /// <value>
        /// The authentication command.
        /// </value>
        public IBearerAuthenticationCommand AuthenticationCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationAttribute" /> class.
        /// </summary>
        public BearerAuthenticationFilter()
        {
            AuthenticationCommand = DependencyResolverContainer.Resolve<IBearerAuthenticationCommand>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationAttribute" /> class.
        /// </summary>
        /// <param name="authenticationCommand">The authentication command.</param>
        public BearerAuthenticationFilter(IBearerAuthenticationCommand authenticationCommand)
        {
            AuthenticationCommand = authenticationCommand;
        }

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
            if (AuthenticationCommand != null)
            {
                if (AuthenticationCommand.SkipAuthorization(context.ActionContext))
                {
                    return;
                }

                IPrincipal principal = null;
                ResponseError errorMessage = AuthorizeResponseMessage.Default;

                foreach (var item in AuthenticationCommand.AuthenticationCommands)
                {
                    principal = await item.AuthenticateAsync(context, cancellationToken);

                    if (principal != null)
                    {
                        if (Configuration.Current.CSRFAttackPrevented && AuthenticationCommand.ValidateCSRFAttack(context))
                        {
                            context.ErrorResult = new AuthenticationFailureResult(context.Request, AuthorizeResponseMessage.InvalidBearerToken);
                            return;
                        }

                        SetPrincipal(context, principal);
                        return;
                    }

                    errorMessage = item.ErrorMessage ?? errorMessage;
                }

                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                // Do not add challenges for the controllers which are passed for do not authenticate
                if (principal == null)
                {
                    context.ErrorResult = context.ErrorResult ?? new AuthenticationFailureResult(context.Request, errorMessage);
                }
            }
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

        /// <summary>
        /// Challenges the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            var challenge = new AuthenticationHeaderValue("Bearer");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
        }

        /// <summary>
        /// Sets the principal.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="principal">The principal.</param>
        private void SetPrincipal(HttpAuthenticationContext context, IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;

            // 6. If the token is valid, set principal.
            if (context != null)
            {
                var claimsPrincipal = principal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult(context.Request, AuthorizeResponseMessage.NoPrincipal);
                    return;
                }
                else if (!(claimsPrincipal is HermesPrincipal))
                {
                    context.Principal = new HermesPrincipal((ClaimsPrincipal)claimsPrincipal);
                }
            }
        }
    }
}
