using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace Hermes.WebApi.Core.Security
{
    /// <summary>
    /// Class to encapsulate the hermes principal.
    /// </summary>
    /// <seealso cref="System.Security.Claims.ClaimsPrincipal" />
    public class HermesPrincipal : ClaimsPrincipal
    {
        /// <summary>
        /// The identity
        /// </summary>
        private readonly HermesIdentity identity;

        /// <summary>
        /// Initializes a new instance of the <see cref="HermesPrincipal" /> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        public HermesPrincipal(HermesIdentity identity) : base(identity)
        {
            this.identity = identity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HermesPrincipal" /> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        public HermesPrincipal(ClaimsIdentity identity) : base(identity)
        {
            this.identity = new HermesIdentity(identity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HermesPrincipal" /> class.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        public HermesPrincipal(ClaimsPrincipal claimsPrincipal) : base(claimsPrincipal)
        {
            identity = new HermesIdentity(claimsPrincipal.Identity);
        }

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        /// <exception cref="System.Security.SecurityException">No current principal</exception>
        public static new HermesPrincipal Current
        {
            get
            {
                HermesPrincipal simplexPrincipal = Thread.CurrentPrincipal as HermesPrincipal;
                if (simplexPrincipal == null)
                {
                    throw new SecurityException("No current principal");
                }

                return simplexPrincipal;
            }
        }

        /// <summary>
        /// Gets the primary claims identity associated with this claims principal.
        /// </summary>
        public override IIdentity Identity
        {
            get
            {
                return identity;
            }
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public IEnumerable<string> Roles
        {
            get
            {
                return FindValues<string>(HermesIdentity.RolesClaimType);
            }
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long UserId
        {
            get
            {
                return FindFirstValue<long>(HermesIdentity.UserIdClaimType);
            }
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get
            {
                return FindFirstValue<string>(HermesIdentity.UsernameClaimType);
            }
        }

        /// <summary>
        /// Gets the name of this claims identity.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return Username;
            }
        }

        /// <summary>
        /// Gets the security identifier.
        /// </summary>
        /// <value>
        /// The security identifier.
        /// </value>
        public Guid SecurityId
        {
            get
            {
                return FindFirstValue<Guid>(HermesIdentity.SIDClaimType);
            }
        }

        /// <summary>
        /// Gets the user authentication token identifier.
        /// </summary>
        /// <value>
        /// The user authentication token identifier.
        /// </value>
        public string UserAuthTokenId
        {
            get
            {
                return FindFirstValue<string>(HermesIdentity.AuthTokenClaimType);
            }
        }

        /// <summary>
        /// Gets the security ids.
        /// </summary>
        /// <value>
        /// The security ids.
        /// </value>
        public IEnumerable<Guid> SecurityIds
        {
            get
            {
                return FindValues<Guid>(HermesIdentity.SecurityIdsClaimType);
            }
        }

        /// <summary>
        /// Returns a value that indicates whether the entity (user) represented by this claims principal is in the specified role.
        /// </summary>
        /// <param name="role">The role for which to check.</param>
        /// <returns>
        /// true if claims principal is in the specified role; otherwise, false.
        /// </returns>
        public override bool IsInRole(string role)
        {
            return this.Roles.Contains(role);
        }

        /// <summary>
        /// Finds the first value.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>type</returns>
        private T FindFirstValue<T>(string type)
        {
            return Claims
                .Where(p => p.Type == type)
                .Select(p => (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(p.Value))
                .FirstOrDefault();
        }

        /// <summary>
        /// Finds the values.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>type collection</returns>
        private IEnumerable<T> FindValues<T>(string type)
        {
            return Claims
                .Where(p => p.Type == type)
                .Select(p => (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(p.Value))
                .ToList();
        }
    }
}