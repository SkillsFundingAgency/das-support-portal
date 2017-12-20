using System.Threading.Tasks;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public interface IClientAuthenticator
    {
        Task<string> Authenticate(string clientId, string appKey, string resourceId, string tenant);
    }
}