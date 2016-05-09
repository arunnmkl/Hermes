using GlobalTranz.Authentication;
using GlobalTranz.WebApi.Core;
using GlobalTranz.WebApi.Core.Filters;
using GlobalTranz.WebApi.Core.Security;
using GlobalTranz.WebApi.Helper.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;

namespace GlobalTranz.WebApi.Test.Dependency
{
	[TestClass]
	public class DependencyTest
	{
		private Uri _baseAddress;
		private HttpSelfHostConfiguration _config;
		private HttpClient _httpClient;

		[TestInitialize]
		public void Init()
		{
			DependencyResolverContainer.RegisterInstance<IAuthentication>(new BasicAuthenticateController());

			_baseAddress = new Uri("http://www.avinash.com");
			_config = new HttpSelfHostConfiguration(_baseAddress);
			_httpClient = new HttpClient(new HttpSelfHostServer(_config));
			_httpClient.BaseAddress = _baseAddress;
			_config.Configure();

			//if (Configuration.Current.BasicAuthenticationEnabled)
			//	_config.Filters.Add(new BasicAuthenticationAttribute());

			if (Configuration.Current.SimplexAuthorizationEnabled)
				_config.Filters.Add(new SimplexAuthorizationAttribute());

			////_config.DependencyResolver = new DependencyResolver();
		}

		[TestMethod]
		public void TestSelftHostApi_ValidateModelAttribute_Invalid()
		{
			var result = _httpClient.GetAsync("Dependency").Result;

			Assert.IsTrue(result.IsSuccessStatusCode, "Status Code: " + result.StatusCode);
		}
	}
}
