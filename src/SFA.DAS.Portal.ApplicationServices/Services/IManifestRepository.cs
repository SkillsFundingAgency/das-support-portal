using System.Collections.Generic;
using SFA.DAS.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Portal.ApplicationServices.Services
{
    public interface IManifestRepository
    {
        bool ChallengeExists(string key);
        SiteChallenge GetChallenge(string key);
        bool ResourceExists(string key);
        SiteResource GetResource(string key);
        string GetResourcePage(string key, string id);
        NavViewModel GetNav(string key, string id);
        object GenerateHeader(string key, string id);
        string GetChallengeForm(string key, string id, string url);
        ChallengeResult SubmitChallenge(string id, IDictionary<string, string> pairs);
        ICollection<SiteManifest> Manifests { get; }
    }
}