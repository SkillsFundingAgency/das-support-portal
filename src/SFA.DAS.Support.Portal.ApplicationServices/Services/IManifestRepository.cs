using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public interface IManifestRepository
    {
        Task<ResourceResultModel> GetResourcePage(SupportServiceResourceKey key, string id);
        Task<NavViewModel> GetNav(SupportServiceResourceKey key, string id);
        Task<ResourceResultModel> GenerateHeader(SupportServiceResourceKey key, string id);
        Task<string> GetChallengeForm(SupportServiceResourceKey resourceKey, SupportServiceResourceKey challengeKey, string id, string url);
        Task<ChallengeResult> SubmitChallenge(string id, IDictionary<string, string> pairs);
    }
}