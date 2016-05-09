using Hermes.WebApi.Core;
using Hermes.WebApi.Core.Filters;
using Hermes.WebApi.Core.Handlers;
using Hermes.WebApi.Core.Interfaces;
using Hermes.WebApi.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.ExceptionHandling;
using GTSecurity = Hermes.WebApi.Core.Security;

namespace Hermes.WebApi.Test.CSRFAttack
{
	[TestClass]
	public class CSRFAttack : TestClassBase, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CSRFAttack"/> class.
		/// </summary>
		public CSRFAttack()
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

			_config.Services.Replace(typeof(IExceptionHandler), new GeneralExceptionHandler());
			_config.Filters.Add(new AnyExceptionFilterAttribute());
			_config.MessageHandlers.Add(new CSRFHandler());
		}

		private string GetCSRFValue(HttpResponseMessage result)
		{
			return result.GetCookie(Hermes.WebApi.Core.Security.Configuration.Current.CSRFCookieName);
		}

		private HttpResponseMessage CallATestServiceFirstToGetTheCSRFValue()
		{
			return _httpClient.PostAsync<Hermes.WebApi.Web.Models.Login>(string.Concat(Config.ApiRouteVersion, "Login"), new Web.Models.Login { Username = _username, Password = _password }, new JsonMediaTypeFormatter()).Result;
		}

		private HttpResponseMessage CallATestServiceFirstToGetTheCSRFValueAnonymous()
		{
			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "AnonymousAuthorization"));
			return _httpClient.SendAsync(request).Result;
		}

		[TestMethod]
		public void CSRFAttack_With_Header_Correct_Username_Password_ButCSRF()
		{
			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Authentication"));
			request.Headers.Add("Authorization", AuthorizationHeader);

			var result = _httpClient.SendAsync(request).Result;

			// basic auth dose not validate the CCRF, because that request can be from browser itself
			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void CSRFAttack_With_Header_Correct_Username_Password_NoCSRF_Attack()
		{
			var response = CallATestServiceFirstToGetTheCSRFValueAnonymous();

			var csrfValue = GetCSRFValue(response);
			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Authentication"));
			request.Headers.Add("Authorization", AuthorizationHeader);
			request.Headers.Add(GTSecurity.Configuration.Current.CSRFHeaderName, csrfValue);
			request.AddCookie(GTSecurity.Configuration.Current.CSRFCookieName, csrfValue);

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Status Code: " + result.StatusCode + "Value : " + csrfValue);
		}

		[TestMethod]
		public void CSRFAttack_With_Header_Correct_Username_Password_NoCSRF_Second()
		{
			var response = CallATestServiceFirstToGetTheCSRFValueAnonymous();

			var csrfValue = GetCSRFValue(response);
			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Authentication"));
			request.Headers.Add("Authorization", AuthorizationHeader);
			request.Headers.Add(GTSecurity.Configuration.Current.CSRFHeaderName, csrfValue);
			request.AddCookie(GTSecurity.Configuration.Current.CSRFCookieName, csrfValue);

			var result = _httpClient.SendAsync(request).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Status Code: " + result.StatusCode + "Value : " + csrfValue + result.ReasonPhrase);
		}

		[TestMethod]
		public void CSRFAttack_With_Header_Correct_Username_Password_NoCSRF_CookieAuth()
		{
			var request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Authentication"));
			request.Headers.Add("Authorization", AuthorizationHeader);

			var result = _httpClient.SendAsync(request).Result;
			var csrfValue = GetCSRFValue(result);

			var cookieRequest = result.GetCookie(GTSecurity.Configuration.Current.AuthCookieName);

			var requestTwo = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Authentication"));
			requestTwo.AddCookie(GTSecurity.Configuration.Current.AuthCookieName, cookieRequest);
			requestTwo.Headers.Add(GTSecurity.Configuration.Current.CSRFHeaderName, csrfValue);
			requestTwo.AddCookie(GTSecurity.Configuration.Current.CSRFCookieName, csrfValue);

			result = _httpClient.SendAsync(requestTwo).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Status Code: " + result.StatusCode + "Value : " + csrfValue + result.ReasonPhrase);
		}

		[TestMethod]
		public void CSRFAttack_With_Header_Correct_Username_Password_CSRF_CookieAuth()
		{
			var request = new HttpRequestMessage(HttpMethod.Get, "Authentication");
			request.Headers.Add("Authorization", AuthorizationHeader);

			var result = _httpClient.SendAsync(request).Result;
			var csrfValue = GetCSRFValue(result);

			var cookieRequest = result.GetCookie(GTSecurity.Configuration.Current.AuthCookieName);

			var requestTwo = new HttpRequestMessage(HttpMethod.Get, string.Concat(Config.ApiRouteVersion, "Authentication"));
			requestTwo.AddCookie(GTSecurity.Configuration.Current.AuthCookieName, cookieRequest);
			requestTwo.AddCookie(GTSecurity.Configuration.Current.CSRFCookieName, csrfValue);

			result = _httpClient.SendAsync(requestTwo).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized, "Status Code: " + result.StatusCode + "Value : " + csrfValue + result.ReasonPhrase);
		}
	}
}