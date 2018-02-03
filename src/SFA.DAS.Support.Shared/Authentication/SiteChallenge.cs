using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Shared.Authentication
{
    [ExcludeFromCodeCoverage]
    public class SiteChallenge
    {
        [JsonRequired] public SupportServiceResourceKey ChallengeKey { get; set; }

        [JsonRequired] public string ChallengeUrlFormat { get; set; }
    }
}