using System.Security.Principal;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public interface IWindowsLogonIdentityProvider
    {
        WindowsIdentity GetIdentity();
    }
}