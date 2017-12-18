using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public interface IActiveDirectoryClientAuthenticator
    {
        Task Authenticate(HttpClient client, string clientId, string appKey, string resourceId, string tenant);
    }
}