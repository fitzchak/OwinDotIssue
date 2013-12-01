using System.Web;
using System.Web.Http;

namespace OwinDotIssue
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}
