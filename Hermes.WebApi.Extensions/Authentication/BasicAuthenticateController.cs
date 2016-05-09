// ***********************************************************************
// Assembly         : Hermes.WebApi.Extensions
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-22-2016
// ***********************************************************************
// <copyright file="BasicAuthenticateController.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using AmadeusConsulting.Simplex.Security;
using Hermes.WebApi.Core.Common;
using Hermes.WebApi.Core.Interfaces;
using System;
using System.Security.Principal;

namespace Hermes.Authentication
{
	/// <summary>
	/// Class BasicAuthenticateController with BasicAuthenticationBase.
	/// </summary>
	public class BasicAuthenticateController : BasicAuthenticationBase
	{
		/// <summary>
		/// Authenticates the username password.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns>IPrincipal.</returns>
		public override IPrincipal AuthenticateUsernamePassword(string userName, string password)
		{
			try
			{
				return AuthenticationCommands.AuthenticateUsernamePassword(userName, password);
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
				return null;
			}
		}
	}
}