using System.Collections.Generic;
using SFA.DAS.Support.Shared.Authentication;

namespace SFA.DAS.Support.Shared.Discovery
{
    public interface ISiteManifest
    {
        string Version { get; }
        IEnumerable<SiteResource> Resources { get; }
        string BaseUrl { get; }
        IEnumerable<SiteChallenge> Challenges { get; }
    }
}