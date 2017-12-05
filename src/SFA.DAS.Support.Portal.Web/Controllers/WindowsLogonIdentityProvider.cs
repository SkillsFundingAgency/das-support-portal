using System.Security.Principal;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public class WindowsLogonIdentityProvider : IWindowsLogonIdentityProvider
    {
        public WindowsIdentity GetIdentity()
        {
            return System.Web.HttpContext.Current?.Request?.LogonUserIdentity?? WindowsIdentity.GetCurrent();
        }
    }
}