using System.Security.Claims;
using System.Web;
using SFA.DAS.Portal.ApplicationServices.Services;

namespace SFA.DAS.Portal.Web.Services
{
    public class IdentityService : IGetCurrentIdentity
    {
        public ClaimsIdentity GetCurrentIdentity()
        {
            return (ClaimsIdentity) HttpContext.Current.User.Identity;
        }
    }
}