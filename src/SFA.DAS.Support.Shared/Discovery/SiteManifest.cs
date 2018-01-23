using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Shared.Authentication;

namespace SFA.DAS.Support.Shared.Discovery
{
    [ExcludeFromCodeCoverage]
    public class SiteManifest : ISiteManifest
    {
        public string Version { get; set; }
        public IEnumerable<SiteResource> Resources { get; set; }
        public string BaseUrl { get; set; }
        public IEnumerable<SiteChallenge> Challenges { get; set; }
    }
}