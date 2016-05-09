using System.Web.Http;

namespace Hermes.WebApi.Web
{
	/// <summary>
	/// Class WebApiApplication.
	/// </summary>
	public class WebApiApplication : System.Web.HttpApplication
	{
		/// <summary>
		/// Application_s the start.
		/// </summary>
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}