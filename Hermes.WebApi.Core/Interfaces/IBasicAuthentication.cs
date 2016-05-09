// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="IBasicAuthentication.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.WebApi.Core.Interfaces
{
	/// <summary>
	/// Interface IBasicAuthentication
	/// </summary>
	public interface IBasicAuthentication : IAuthentication
	{
		/// <summary>
		/// Authenticates the username password.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns>IPrincipal.</returns>
		IPrincipal AuthenticateUsernamePassword(string userName, string password);
	}
}
