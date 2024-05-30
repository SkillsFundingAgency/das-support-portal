using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public interface IManifestRepository
    {
        Task<ResourceResultModel> GetResourcePage(SupportServiceResourceKey key, string id, string childId, string supportUserEmail);
        Task<NavViewModel> GetNav(SupportServiceResourceKey key, string id);
        Task<ResourceResultModel> GenerateHeader(SupportServiceResourceKey key, string id);
        Task<string> GetChallengeForm(SupportServiceResourceKey resourceKey, SupportServiceResourceKey challengeKey, string id, string url);
        Task<ChallengeResult> SubmitChallenge(string id, IDictionary<string, string> pairs);

        Task<ResourceResultModel> SubmitApprenticeSearchRequest(SupportServiceResourceKey key, string hashedAccountId,ApprenticeshipSearchType searchType,string searchTerm);
        Task SubmitChangeRoleRequest(string hashedAccountId,string userRef, string role, string supportUserEmail);
        Task SubmitCreateInvitationRequest(string hashedAccountId, string email, string fullName, string supportUserEmail, string role);
    }
}