using AmadeusConsulting.Simplex.Security;
using Hermes.WebApi.Core;
using Hermes.WebApi.Core.Filters;
using Hermes.WebApi.Core.Handlers;
using Hermes.WebApi.Core.Interfaces;
using Hermes.WebApi.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using GTSecurity = Hermes.WebApi.Core.Security;

namespace Hermes.Authentication
{
	[TestClass]
	public class AuthenticationTest : TestClassBase, IDisposable
	{
		#region Initializes Test

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationTest"/> class.
		/// </summary>
		public AuthenticationTest()
		{
			Init();
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected override void Init()
		{
			base.Init();
			if (Hermes.WebApi.Core.Security.Configuration.Current.AuthenticationEnabled)
				_config.Filters.Add(new AuthenticationAttribute(DependencyResolverContainer.Resolve<IAuthenticationCommand>()));

			if (Hermes.WebApi.Core.Security.Configuration.Current.HermesAuthorizationEnabled)
				_config.Filters.Add(new HermesAuthorizationAttribute());

			_config.MessageHandlers.Add(new CSRFHandler());
		}

		#endregion Initializes Test

		[TestMethod]
		public void Authentication_No_Authentication_Header()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "Authentication")).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void Authentication_With_Header_No_Username_Password()
		{
			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Authentication"));
			request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", string.Empty, string.Empty))));

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void Authentication_With_Header_Correct_Username_Password()
		{
			UserGroupController.ClearTestUserData();
			UserGroupController.CreateUser();

			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Authentication"));
			request.Headers.Add("Authorization", AuthorizationHeader);

			// This is for the SCRF attack
			request.Headers.Add(GTSecurity.Configuration.Current.CSRFHeaderName, "12345");
			request.AddCookie(GTSecurity.Configuration.Current.CSRFCookieName, "12345");

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void Authentication_With_Header_Incorrect_Username_Password()
		{
			UserGroupController.ClearTestUserData();
			UserGroupController.CreateUser();

			_username = "test";
			_password = "testPassword";

			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Authentication"));
			request.Headers.Add("Authorization", AuthorizationHeader);

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void Authorization_With_Correct_RoleGroup()
		{
			UserGroupController.ClearTestUserData();
			UserGroupController.CreateUser(addGroup: true);

			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "RoleAuthorization"));
			request.Headers.Add("Authorization", AuthorizationHeader);

			// This is for the SCRF attack
			request.Headers.Add(GTSecurity.Configuration.Current.CSRFHeaderName, "12345");
			request.AddCookie(GTSecurity.Configuration.Current.CSRFCookieName, "12345");

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Reason Phrase: " + result.ReasonPhrase);
		}

		[TestMethod]
		public void Authorization_With_Incorrect_RoleGroup()
		{
			UserGroupController.ClearTestUserData();
			UserGroupController.CreateUser();

			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "RoleAuthorization"));
			request.Headers.Add("Authorization", AuthorizationHeader);

			// This is for the SCRF attack
			request.Headers.Add(GTSecurity.Configuration.Current.CSRFHeaderName, "12345");
			request.AddCookie(GTSecurity.Configuration.Current.CSRFCookieName, "12345");

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized, "Reason Phrase: " + result.ReasonPhrase);
		}

		[TestMethod]
		public void Authorization_Anonymous_User()
		{
			UserGroupController.ClearTestUserData();

			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "AnonymousAuthorization"));
			request.Headers.Add("Authorization", AuthorizationHeader);

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void Authorization_SimplexAuthorization_Unauthorized()
		{
			UserGroupController.ClearTestUserData();

			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "SimplexAuthorization"));
			request.Headers.Add("Authorization", AuthorizationHeader);

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized, "Reason Phrase: " + result.ReasonPhrase);
		}

		[TestMethod]
		public void Authorization_SimplexAuthorization_Authorized()
		{
			UserGroupController.ClearTestUserData();
			UserGroupController.CreateUser(addGroup: true, createResource: true);

			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "SimplexAuthorization"));
			request.Headers.Add("Authorization", AuthorizationHeader);

			// This is for the SCRF attack
			request.Headers.Add(GTSecurity.Configuration.Current.CSRFHeaderName, "12345");
			request.AddCookie(GTSecurity.Configuration.Current.CSRFCookieName, "12345");

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void Authentication_With_Cookies_Success()
		{
			UserGroupController.ClearTestUserData();
			UserGroupController.CreateUser(addGroup: true, createResource: true);

			var principal = AuthenticationCommands.AuthenticateUsernamePassword(_username, _password);

			var cookieValue = principal.Identity.SecureTicketString;
			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Cookie"));
			request.AddCookie(GTSecurity.Configuration.Current.AuthCookieName, HttpUtility.UrlEncode(cookieValue));

			// This is for the SCRF attack
			request.Headers.Add(GTSecurity.Configuration.Current.CSRFHeaderName, "12345");
			request.AddCookie(GTSecurity.Configuration.Current.CSRFCookieName, "12345");

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void Authentication_With_Cookies_Invalid()
		{
			var cookieValue = "Invalid ticket";
			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Cookie"));
			request.Headers.Add("Cookie", string.Format("{1}={0};", HttpUtility.UrlEncode(cookieValue), GTSecurity.Configuration.Current.AuthCookieName));

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized, "Reason Phrase: " + result.ReasonPhrase);
		}
	}
}