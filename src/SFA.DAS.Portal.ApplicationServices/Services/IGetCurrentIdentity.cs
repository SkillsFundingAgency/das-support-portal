using System.Security.Claims;

namespace SFA.DAS.Portal.ApplicationServices.Services
{
    public interface IGetCurrentIdentity
    {
        ClaimsIdentity GetCurrentIdentity();
    }
}