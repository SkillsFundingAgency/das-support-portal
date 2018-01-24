using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Web;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class WindowsLogonIdentityProvider : IWindowsLogonIdentityProvider
    {
        public WindowsIdentity GetIdentity()
        {
            return HttpContext.Current?.Request?.LogonUserIdentity ?? WindowsIdentity.GetCurrent();
        }
    }
}