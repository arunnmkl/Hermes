using Hermes.WebApi.Core;
using Hermes.WebApi.Core.Handlers;
using Hermes.WebApi.Core.Services;
using Hermes.WebApi.Extensions.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Web.Http.ExceptionHandling;

namespace Hermes.WebApi.Test.ApiKeyValidation
{
	[TestClass]
	public class ApiKeyValidationTest : TestClassBase, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApiKeyValidationTest"/> class.
		/// </summary>
		public ApiKeyValidationTest()
		{
			Init();
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected override void Init()
		{
			base.Init();
			_config.MessageHandlers.Add(new ApiKeyHandler(new ApiKeyValidator("TestAPIIsValid")));
			_config.Services.Replace(typeof(IExceptionLogger), new GlobalExceptionLogger());
		}

		[TestMethod]
		public void ApiKeyValidator_Valid_Api_Key()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "KeyValidation?key=TestAPIIsValid")).Result;

			//Assert.IsFalse(result.IsSuccessStatusCode, "Status Code: " + result.StatusCode);
			Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "Status Code: " + result.StatusCode);
		}

		[TestMethod]
		public void ApiKeyValidator_InValid_Api_Key()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "KeyValidation?key=TestAPIIsValid1")).Result;

			//Assert.IsFalse(result.IsSuccessStatusCode, "Status Code: " + result.StatusCode);
			Assert.AreEqual(result.StatusCode, HttpStatusCode.Forbidden, "Status Code: " + result.StatusCode);
		}
	}
}