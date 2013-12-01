using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;
using Owin;

[assembly: OwinStartup(typeof(OwinDotIssue.Startup))]

namespace OwinDotIssue
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);

			app.UseWelcomePage(new WelcomePageOptions { Path = new PathString() { } });
		}
	}
}