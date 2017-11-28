using System.Security.Principal;
using System.Web;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public interface ICheckPermissions
    {
        bool HasPermissions(HttpRequestBase request, HttpResponseBase response, IPrincipal user, string id);
    }
}