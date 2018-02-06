using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public interface IManifestRepository
    {
        Task<bool> ChallengeExists(SupportServiceResourceKey key);
        Task<SiteChallenge> GetChallenge(SupportServiceResourceKey key);
        Task<bool> ResourceExists(SupportServiceResourceKey key);
        Task<SiteResource> GetResource(SupportServiceResourceKey key);
        Task<ResourceResultModel> GetResourcePage(SupportServiceResourceKey key, string id);
        Task<NavViewModel> GetNav(SupportServiceResourceKey key, string id);
        Task<ResourceResultModel> GenerateHeader(SupportServiceResourceKey key, string id);
        Task<string> GetChallengeForm(SupportServiceResourceKey key, string id, string url);
        Task<ChallengeResult> SubmitChallenge(string id, IDictionary<string, string> pairs);
    }
}