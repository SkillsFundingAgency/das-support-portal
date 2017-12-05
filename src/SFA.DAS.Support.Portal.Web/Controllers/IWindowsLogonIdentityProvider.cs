using System.Security.Principal;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public interface IWindowsLogonIdentityProvider
    {
        WindowsIdentity GetIdentity();
    }
}