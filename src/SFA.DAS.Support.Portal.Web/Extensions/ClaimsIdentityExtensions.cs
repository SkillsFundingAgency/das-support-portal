using System.Linq;
using System.Security.Claims;

namespace SFA.DAS.Support.Portal.Web.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static string GetClaimValue(this ClaimsIdentity claimsIdentity, string claimName)
        {
            var claimValue = claimsIdentity.Claims.Where(c => c.Type.Contains(claimName)).Select(c => c.Value).SingleOrDefault();
            return claimValue ?? string.Empty;
        }
    }
}