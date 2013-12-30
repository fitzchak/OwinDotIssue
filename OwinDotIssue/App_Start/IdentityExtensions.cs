using System;
using System.Security.Claims;
using System.Security.Principal;

namespace OwinDotIssue
{
	public static class IdentityExtensions
	{
		public static string GetUserName(this IIdentity identity)
		{
			if (identity == null)
				throw new ArgumentNullException("identity");

			var identity1 = identity as ClaimsIdentity;
			if (identity1 != null)
				return identity1.FindFirstValue(ClaimTypes.Name);
			return null;
		}

		public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
		{
			if (identity == null)
				throw new ArgumentNullException("identity");

			var first = identity.FindFirst(claimType);
			if (first != null)
				return first.Value;
			return null;
		}
	}
}