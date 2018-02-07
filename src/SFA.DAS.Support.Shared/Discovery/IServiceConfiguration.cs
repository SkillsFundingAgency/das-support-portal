using System;
using System.Collections.Generic;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Navigation;

namespace SFA.DAS.Support.Shared.Discovery
{
    public interface IServiceConfiguration
    {
        bool ResourceExists(SupportServiceResourceKey key);
        bool ChallengeExists(SupportServiceResourceKey key);
        SiteResource GetResource(SupportServiceResourceKey key);
        SiteChallenge GetChallenge(SupportServiceResourceKey key);
        IEnumerable<NavItem> GetNavItems(SupportServiceResourceKey key, string id);
        SiteManifest ManifestFromResource(SiteResource resource);
        SiteResource FindResource(SupportServiceResourceKey key);

        Uri FindSiteBaseUriForManfiestElement(Dictionary<SupportServiceIdentity, Uri> sites,
            SupportServiceResourceKey challengeKey);
    }
    
}