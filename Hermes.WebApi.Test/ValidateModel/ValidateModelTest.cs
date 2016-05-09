using Hermes.Authentication;
using Hermes.WebApi.Core;
using Hermes.WebApi.Core.Enums;
using Hermes.WebApi.Core.Filters;
using Hermes.WebApi.Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;

namespace Hermes.WebApi.Test.ValidateModelTest
{
	[TestClass]
	public class ValidateModelTest : IDisposable
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

			_config.Filters.Add(new ValidateModelAttribute());
		}

		[TestMethod]
		public void ValidateModelAttribute_Invalid_Input()
		{
			var result = _httpClient.PostAsync<TestModel>(string.Concat(Config.ApiRouteVersion, "ValidateModel"), new TestModel(), new JsonMediaTypeFormatter()).Result;

			Assert.IsFalse(result.IsSuccessStatusCode, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void ValidateModelAttribute_Valid_Input()
		{
			var result = _httpClient.PostAsync<TestModel>(string.Concat(Config.ApiRouteVersion, "ValidateModel"), new TestModel { Name = "Test" }, new JsonMediaTypeFormatter()).Result;

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
