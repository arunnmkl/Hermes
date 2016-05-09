using Hermes.WebApi.Core;
using Hermes.WebApi.Core.Handlers;
using Hermes.WebApi.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Web.Http.ExceptionHandling;

namespace Hermes.WebApi.Test.ErrorLog
{
	[TestClass]
	public class ErrorTest : TestClassBase, IDisposable
	{
		public ErrorTest()
		{
			Init();
			_config.MessageHandlers.Add(new CSRFHandler());
			_config.Services.Replace(typeof(IExceptionHandler), new GeneralExceptionHandler());
			_config.Services.Replace(typeof(IExceptionLogger), new GlobalExceptionLogger());
		}

		protected override void Init()
		{
			base.Init();
			_config.MessageHandlers.Add(new CSRFHandler());
			_config.Services.Replace(typeof(IExceptionHandler), new GeneralExceptionHandler());
			_config.Services.Replace(typeof(IExceptionLogger), new GlobalExceptionLogger());
		}

		[TestMethod]
		public void Authentication_ErrorTest()
		{
			var result = _httpClient.GetAsync(string.Concat(Config.ApiRouteVersion, "ErrorResult")).Result;

			// have to make sure that the error log is getting called
			Assert.AreEqual(result.StatusCode, HttpStatusCode.InternalServerError, "Status Code: " + result.StatusCode);
		}
	}
}