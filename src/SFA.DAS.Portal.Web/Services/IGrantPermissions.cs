using System.Security.Principal;
using System.Web;

namespace SFA.DAS.Portal.Web.Services
{
    public interface IGrantPermissions
    {
        void GivePermissions(HttpResponseBase response, IPrincipal user, string id);
    }
}