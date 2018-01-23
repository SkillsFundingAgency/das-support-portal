using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public interface IManifestRepository
    {
        Task<bool> ChallengeExists(string key);
        Task<SiteChallenge> GetChallenge(string key);
        Task<bool> ResourceExists(string key);
        Task<SiteResource> GetResource(string key);
        Task<string> GetResourcePage(string key, string id);
        Task<NavViewModel> GetNav(string key, string id);
        Task<object> GenerateHeader(string key, string id);
        Task<string> GetChallengeForm(string key, string id, string url);
        Task<ChallengeResult> SubmitChallenge(string id, IDictionary<string, string> pairs);
        Task<List<SiteManifest>> GetManifests();
    }
}