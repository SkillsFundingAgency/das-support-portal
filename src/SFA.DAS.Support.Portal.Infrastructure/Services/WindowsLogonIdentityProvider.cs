using System.Security.Principal;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public class WindowsLogonIdentityProvider : IWindowsLogonIdentityProvider
    {
        public WindowsIdentity GetIdentity()
        {
            return System.Web.HttpContext.Current?.Request?.LogonUserIdentity?? WindowsIdentity.GetCurrent();
        }
    }
}