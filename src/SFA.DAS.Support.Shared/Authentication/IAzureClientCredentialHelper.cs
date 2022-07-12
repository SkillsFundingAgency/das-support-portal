using System.Threading.Tasks;

namespace SFA.DAS.Support.Shared.Authentication
{
    public interface IAzureClientCredentialHelper
    {
        Task<string> GetAccessTokenAsync(string identifier);
    }
}
