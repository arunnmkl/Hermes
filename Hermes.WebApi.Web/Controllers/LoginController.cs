// ***********************************************************************
// Assembly         : Hermes.WebApi.Web
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="LoginController.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary>This file has a controller which is used for the authentication.</summary>
// ***********************************************************************
using AmadeusConsulting.Simplex.Security;
using Hermes.WebApi.Core;
using Hermes.WebApi.Web.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hermes.WebApi.Web.Controllers
{
	[RoutePrefix("API/Controllers")]
	[AllowAnonymous]
	public class LoginController : ApiController
	{

		//[Route("Login")]
		public HttpResponseMessage Get()
		{
			return Request.CreateResponse(true);
		}

		[Route("Login/{id}")]
		public HttpResponseMessage Get(int id)
		{
			return Request.CreateResponse(id);
		}

		//public HttpResponseMessage Get(string name)
		//{
		//	return Request.CreateResponse(name);
		//}

		/// <summary>
		/// Posts the specified login object to authentication of the API.
		/// </summary>
		/// <param name="login">The login object with the username and password.</param>
		/// <returns><c>HttpStatusCode.OK</c> if [login success]; otherwise, <c>HttpStatusCode.Unauthorized</c>.</returns>
		public HttpResponseMessage Post(Login login)
		{
			string loginMessage = string.Empty;
			try
			{
				var principal = AuthenticationCommands.AuthenticateUsernamePassword(login.Username, login.Password);

				if (principal != null)
				{
					loginMessage = "Login was successful!";
					var response = Request.CreateResponse(HttpStatusCode.OK, true);
					response.SetAuthentication(principal.Identity.SecureTicketString, login.RememberMe);

					return response;
				}
			}
			catch (Exception ex)
			{
				loginMessage = ex.ToString();
			}

			return Request.CreateResponse(HttpStatusCode.Unauthorized, loginMessage);
		}
	}
}