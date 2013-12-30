using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace OwinDotIssue
{
	public class ApplicationOAuthBearerProvider : OAuthBearerAuthenticationProvider
	{
		public override Task ValidateIdentity(OAuthValidateIdentityContext context)
		{
			if (context.Ticket.Identity.Claims.Any() == false)
				context.Rejected();
			else if (context.Ticket.Identity.Claims.All(c => c.Issuer == ClaimsIdentity.DefaultIssuer))
				context.Rejected();
			return Task.FromResult<object>(null);
		}
	}
}