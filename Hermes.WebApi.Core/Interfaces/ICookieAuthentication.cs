// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="ICookieAuthentication.cs" company="">
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
	/// Interface ICookieAuthentication
	/// </summary>
	public interface ICookieAuthentication : IAuthentication
	{
		/// <summary>
		/// Authenticates the cookie.
		/// </summary>
		/// <param name="ticket">The ticket.</param>
		/// <returns>IPrincipal.</returns>
		IPrincipal AuthenticateCookie(string ticket);
	}
}
