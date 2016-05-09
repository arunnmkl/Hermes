using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.SelfHost;
using Hermes.Authentication;
using Hermes.WebApi.Core;
using Hermes.WebApi.Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermes.WebApi.Test
{
	[TestClass]
	public class HttpActionResultTest : IDisposable
	{
		private Uri _baseAddress;
		private HttpSelfHostConfiguration _config;
		private HttpClient _httpClient;

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		[TestInitialize]
		public void Init()
		{
			DependencyResolverContainer.RegisterInstance<IAuthentication>(new BasicAuthenticateController());

			_baseAddress = new Uri("http://www.avinash.com");
			_config = new HttpSelfHostConfiguration(_baseAddress);
			_httpClient = new HttpClient(new HttpSelfHostServer(_config));
			_httpClient.BaseAddress = _baseAddress;

			_config.Configure();
		}

		[TestMethod]
		public void TestSelftHostApi()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "Test")).Result;

			Assert.IsTrue(result.IsSuccessStatusCode, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void TestSelftHostApi_Result()
		{
			var response = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "Test?number=123")).Result;

			string jsonContent = response.Content.ReadAsStringAsync().Result;

			Assert.AreEqual(jsonContent, "123");
		}

		[TestMethod]
		public void TestSelftHostApi_NotFound()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "Test?number=0")).Result;

			Assert.AreEqual(result.StatusCode, HttpStatusCode.NotFound);
		}

		[TestMethod]
		public void TestSelftHostApi_Products()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion,"Test?product=12")).Result;

			Assert.IsTrue(result.IsSuccessStatusCode, result.StatusCode.ToString());
		}

		[TestMethod]
		public void TestSelftHostApi_Put()
		{
			var result = _httpClient.PutAsJsonAsync(string.Concat(Config.ApiRouteVersion, "Test/PutProduct?productName=productName"), "productName").Result;

			Assert.IsTrue(result.IsSuccessStatusCode, result.StatusCode.ToString());
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_config.Dispose();
			_httpClient.Dispose();
			_baseAddress = null;
		}
	}
}