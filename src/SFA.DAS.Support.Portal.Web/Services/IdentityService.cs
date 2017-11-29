using System.Security.Claims;
using System.Web;
using SFA.DAS.Support.Portal.ApplicationServices.Services;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public class IdentityService : IGetCurrentIdentity
    {
        public ClaimsIdentity GetCurrentIdentity()
        {
            return (ClaimsIdentity) HttpContext.Current.User.Identity;
        }
    }
}