using System.Security.Claims;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public interface IGetCurrentIdentity
    {
        ClaimsIdentity GetCurrentIdentity();
    }
}