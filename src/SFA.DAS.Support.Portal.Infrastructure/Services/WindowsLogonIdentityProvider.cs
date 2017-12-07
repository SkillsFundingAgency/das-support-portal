using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class WindowsLogonIdentityProvider : IWindowsLogonIdentityProvider
    {
        public WindowsIdentity GetIdentity()
        {
            return System.Web.HttpContext.Current?.Request?.LogonUserIdentity?? WindowsIdentity.GetCurrent();
        }
    }
}