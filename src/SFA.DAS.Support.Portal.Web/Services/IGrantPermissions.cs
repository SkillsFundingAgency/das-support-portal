using System.Security.Principal;
using System.Web;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public interface IGrantPermissions
    {
        void GivePermissions(HttpResponseBase response, IPrincipal user, string id);
    }
}