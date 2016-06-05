using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Hermes.WebApi.Core.Security
{
    /// <summary>
    /// Class to encapsulate the hermes identity.
    /// </summary>
    /// <seealso cref="System.Security.Claims.ClaimsIdentity" />
    public class HermesIdentity : ClaimsIdentity
    {
        /// <summary>
        /// The roles claim type
        /// </summary>
        public const string RolesClaimType = "http://schemas.dreamorbit.com/Hermes.Security.Role";

        /// <summary>
        /// The user identifier claim type
        /// </summary>
        public const string UserIdClaimType = "http://schemas.dreamorbit.com/Hermes.Security.Id";

        /// <summary>
        /// The security ids claim type
        /// </summary>
        public const string SecurityIdsClaimType = "http://schemas.dreamorbit.com/Hermes.Security.SIDs";

        /// <summary>
        /// The authentication token claim type
        /// </summary>
        public const string AuthTokenClaimType = "http://schemas.dreamorbit.com/Hermes.Security.UserAuthToken";

        /// <summary>
        /// The username claim type
        /// </summary>
        public const string UsernameClaimType = "http://schemas.dreamorbit.com/Hermes.Security.UserName";

        /// <summary>
        /// The sid claim type
        /// </summary>
        public const string SIDClaimType = "http://schemas.dreamorbit.com/Hermes.Security.SID";

        /// <summary>
        /// The authentication client claim type
        /// </summary>
        public const string AuthClientClaimType = "http://schemas.dreamorbit.com/Hermes.Security.AuthClient";

        /// <summary>
        /// Initializes a new instance of the <see cref="HermesIdentity" /> class.
        /// </summary>
        /// <param name="claims">The claims with which to populate the claims identity.</param>
        /// <param name="authenticationType">The type of authentication used.</param>
        public HermesIdentity(IEnumerable<Claim> claims, string authenticationType) : base(authenticationType: authenticationType)
        {
            // Username
            AddClaims(from name in claims where name.Type == UsernameClaimType select name);

            // UserId
            AddClaims(from userId in claims where userId.Type == UserIdClaimType select userId);

            // Security Id's
            AddClaims(from sids in claims where sids.Type == SecurityIdsClaimType select sids);

            // SID
            AddClaims(from sid in claims where sid.Type == SIDClaimType select sid);

            // Authentication Token
            AddClaims(from authToken in claims where authToken.Type == AuthTokenClaimType select authToken);

            // Roles
            AddClaims(from role in claims where role.Type == RolesClaimType select role);

            // AuthClient
            AddClaims(from authClient in claims where authClient.Type == AuthClientClaimType select authClient);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HermesIdentity"/> class.
        /// </summary>
        /// <param name="identity">The identity from which to base the new claims identity.</param>
        public HermesIdentity(IIdentity identity) : base(identity) { }

        /// <summary>
        /// Gets the name of this claims identity.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return Convert.ToString(FindFirst(HermesIdentity.UsernameClaimType).Value);
            }
        }
    }
}