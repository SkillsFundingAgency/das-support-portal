using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace SFA.DAS.Support.Portal.Web.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static string GetClaimValue(this ClaimsIdentity claimsIdentity, string claimName)
        {
            var claimValue = claimsIdentity.Claims.Where(c => c.Type.Contains(claimName)).Select(c => c.Value).SingleOrDefault();
            return claimValue ?? string.Empty;
        }
        
        public static string FindFirstValue(this IPrincipal principal, string claimType)
        {
            return ((ClaimsPrincipal)principal).FindFirstValue(claimType);
        }
        
        private static string FindFirstValue(this ClaimsPrincipal principal, string claimType)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(claimType);
            return claim?.Value;
        }
    }
}