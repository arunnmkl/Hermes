using AmadeusConsulting.Simplex.Base.Serialization;
using AmadeusConsulting.Simplex.Security;
using Hermes.Authentication;
using Hermes.WebApi.Core;
using Hermes.WebApi.Core.Filters;
using Hermes.WebApi.Core.Handlers;
using Hermes.WebApi.Core.Interfaces;
using Hermes.WebApi.Core.Services;
using Hermes.WebApi.Extensions.Authentication;
using Hermes.WebApi.Extensions.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.SelfHost;

namespace Hermes.WebApi.Test
{
	public abstract class TestClassBase
	{
		protected string _username = "unittest";
		protected string _password = "unittestpassword";
		protected Uri _baseAddress;
		protected HttpSelfHostConfiguration _config;
		protected HttpClient _httpClient;

		protected string AuthorizationHeader
		{
			get
			{
				return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _username, _password)));
			}
		}

		protected virtual void Init()
		{
			UserGroupController.ClearTestUserData();
			UserGroupController.CreateUser();

			DependencyResolverContainer.RegisterInstance<IAuthentication>(new AuthenticationAttribute(DependencyResolverContainer.Resolve<IAuthenticationCommand>()));
			DependencyResolverContainer.RegisterInstance<IAuthenticationCommand>(new AuthenticationCommand());
			DependencyResolverContainer.RegisterInstance<IAuthorization>(new AuthorizationController());
			DependencyResolverContainer.RegisterInstance<ICSRFValidation>(new CSRFValidation());
			DependencyResolverContainer.RegisterInstance<IApiKeyValidator>(new ApiKeyValidator("TestAPIIsValid"));

			SimplexTicket.Key = ConfigurationManager.AppSettings["SimplexEncryptionKey"].HexToByteArray();
			SimplexTicket.Iv = ConfigurationManager.AppSettings["SimplexEncryptionIv"].HexToByteArray();

			_baseAddress = new Uri("http://www.avinash.com");
			_config = new HttpSelfHostConfiguration(_baseAddress);
			_httpClient = new HttpClient(new HttpSelfHostServer(_config));
			_httpClient.BaseAddress = _baseAddress;

			_config.Configure();
		}

		public virtual void Dispose()
		{
			_config.Dispose();
			_httpClient.Dispose();
			_baseAddress = null;

			try
			{
				UserGroupController.ClearTestUserData();
			}
			catch (Exception)
			{
			}
		}
	}
}