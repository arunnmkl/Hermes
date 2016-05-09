using Hermes.WebApi.Core.Exceptions;
using Hermes.WebApi.Core.Interfaces;

// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-20-2016
// ***********************************************************************
// <copyright file="HermesAuthorizationAttribute.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Hermes.WebApi.Core.Filters
{
	/// <summary>
	/// Class HermesAuthorizationAttribute.
	/// </summary>
	public class HermesAuthorizationAttribute : AuthorizeAttribute
	{
		#region Properties

		/// <summary>
		/// Gets the name of the resource.
		/// </summary>
		/// <value>The name of the resource.</value>
		protected string ResourceName { get; private set; }

		/// <summary>
		/// Gets the permission.
		/// </summary>
		/// <value>The permission.</value>
		protected string Permission { get; private set; }

		/// <summary>
		/// Gets the action.
		/// </summary>
		/// <value>The action.</value>
		protected string Action { get; private set; }

		/// <summary>
		/// Gets or sets the authentication.
		/// </summary>
		/// <value>The authentication.</value>
		protected IAuthorization Authentication { get; private set; }

		#endregion Properties

		#region Initializes

		/// <summary>
		/// Initializes a new instance of the <see cref="HermesAuthorizationAttribute" /> class.
		/// </summary>
		public HermesAuthorizationAttribute()
			: base()
		{
			Authentication = DependencyResolverContainer.Resolve<IAuthorization>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HermesAuthorizationAttribute" /> class.
		/// </summary>
		/// <param name="authentication">The authentication.</param>
		public HermesAuthorizationAttribute(IAuthorization authentication)
			: base()
		{
			Authentication = authentication ?? DependencyResolverContainer.Resolve<IAuthorization>();
			CheckAuthorizationDependency();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HermesAuthorizationAttribute" /> class.
		/// </summary>
		/// <param name="logicalResourceName">Name of the logical resource.</param>
		/// <param name="permission">The permission.</param>
		public HermesAuthorizationAttribute(string logicalResourceName, string permission)
		{
			Authentication = DependencyResolverContainer.Resolve<IAuthorization>();
			ResourceName = logicalResourceName;
			Permission = permission;
			CheckAuthorizationDependency();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HermesAuthorizationAttribute" /> class.
		/// </summary>
		/// <param name="logicalResourceName">Name of the logical resource.</param>
		/// <param name="action">The action.</param>
		/// <param name="permission">The permission.</param>
		public HermesAuthorizationAttribute(string logicalResourceName, string action, string permission)
		{
			Authentication = DependencyResolverContainer.Resolve<IAuthorization>();
			ResourceName = logicalResourceName;
			Permission = permission;
			Action = action;
			CheckAuthorizationDependency();
		}

		#endregion Initializes

		#region Protected Methods

		/// <summary>
		/// Processes requests that fail authorization.
		/// </summary>
		/// <param name="actionContext">The context.</param>
		protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
		{
			actionContext.Response = Authentication.GetUnauthorizedResponse(actionContext);
		}

		/// <summary>
		/// Indicates whether the specified control is authorized.
		/// </summary>
		/// <param name="actionContext">The context.</param>
		/// <returns>true if the control is authorized; otherwise, false.</returns>
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			return Authentication.IsAuthorized(actionContext, ResourceName, Permission, Action);
		}

		#endregion Protected Methods

		#region Public Methods

		/// <summary>
		/// Calls when an action is being authorized.
		/// </summary>
		/// <param name="actionContext">The context.</param>
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			if (Authentication.SkipAuthorization(actionContext))
				return;

			if (!IsAuthorized(actionContext))
			{
				HandleUnauthorizedRequest(actionContext);
			}
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Checks the authorization dependency.
		/// </summary>
		/// <exception cref="Hermes.WebApi.Core.Exceptions.HermesException">Please implement a dependency IAuthorization</exception>
		private void CheckAuthorizationDependency()
		{
			if (Authentication == null)
			{
				throw new HermesException("Please implement a dependency of IAuthorization");
			}
		}

		#endregion Private Methods
	}
}