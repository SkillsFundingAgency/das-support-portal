using System.Threading.Tasks;

namespace SFA.DAS.Support.Shared.Authentication
{
    public interface IClientAuthenticator
    {
        Task<string> Authenticate(string clientId, string appKey, string resourceId, string tenant);
    }
}