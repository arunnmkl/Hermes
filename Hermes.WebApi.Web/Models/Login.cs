// ***********************************************************************
// Assembly         : Hermes.WebApi.Web
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-22-2016
// ***********************************************************************
// <copyright file="Login.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary>This file contains the login class.</summary>
// ***********************************************************************

namespace Hermes.WebApi.Web.Models
{
	/// <summary>
	/// Class Login object.
	/// </summary>
	public class Login
	{
		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		public string Username { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [remember me].
		/// </summary>
		/// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
		public bool RememberMe { get; set; }
	}
}