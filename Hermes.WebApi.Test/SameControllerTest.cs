using System;
using System.Net.Http;
using System.Web.Http.SelfHost;
using Hermes.Authentication;
using Hermes.WebApi.Core;
using Hermes.WebApi.Core.Enums;
using Hermes.WebApi.Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermes
{
	[TestClass]
	public class SameControllerTest : IDisposable
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
			_config.Configure(routingConfig: RoutingConfig.Namespace);
		}

		[TestMethod]
		public void TestSelftHostApi_VersionSameException()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "SameTest")).Result;

			Assert.IsFalse(result.IsSuccessStatusCode, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void TestSelftHostApi_VersionOne()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "v1/SameTest")).Result;

			Assert.IsTrue(result.IsSuccessStatusCode, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void TestSelftHostApi_VersionTwo()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "v2/SameTest")).Result;

			Assert.IsTrue(result.IsSuccessStatusCode, "Status Code: " + result.StatusCode);
		}

		public void Dispose()
		{
			_config.Dispose();
			_httpClient.Dispose();
			_baseAddress = null;
		}
	}
}