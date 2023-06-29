using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Navigation;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Discovery
{
    public class ServiceConfiguration : List<SiteManifest>, IServiceConfiguration
    {
        public SubSiteConnectorConfig FindSiteConfigForManfiestElement(List<SubSiteConnectorConfig> sites, SupportServiceResourceKey key)
        {
            if (sites == null) throw new ArgumentNullException(nameof(sites));
            
            SubSiteConnectorConfig site;
            foreach (var item in this)
            {
                if (item.Challenges.Any(c => c.ChallengeKey == key))
                {
                    var itemChallengeResourceKey = item.Challenges.First(y => y.ChallengeKey == key).ServiceIdentity.ToString();
                    site = sites.FirstOrDefault(x => x.Key.Equals(itemChallengeResourceKey, StringComparison.InvariantCultureIgnoreCase));
                    return site;
                }

                if (item.Resources.Any(c => c.ResourceKey == key))
                {
                    var itemChallengeResourceKey = item.Resources.First(y => y.ResourceKey == key).ServiceIdentity.ToString();
                    site = sites.FirstOrDefault(x => x.Key.Equals(itemChallengeResourceKey, StringComparison.InvariantCultureIgnoreCase));
                    return site;
                }
            }

            return null;
        }

        public bool ResourceExists(SupportServiceResourceKey key)
        {
            var resource = FindResource(key);

            return resource != null;
        }

        public bool ChallengeExists(SupportServiceResourceKey key)
        {
            foreach (var manifest in this)
            {
                if (manifest.Challenges.Any(challenge => challenge.ChallengeKey == key))
                {
                    return true;
                }
            }
            return false;
        }

        public SiteResource GetResource(SupportServiceResourceKey key)
        {
            var resource = FindResource(key);
            return resource;
        }

        private SiteChallenge FindChallenge(SupportServiceResourceKey key)
        {
            foreach (var manifest in this)
            {
                foreach (var item in manifest.Challenges)
                {
                    if (item.ChallengeKey == key)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public SiteChallenge GetChallenge(SupportServiceResourceKey key)
        {
            var challenge = FindChallenge(key);
            return challenge;
        }

        public IEnumerable<NavItem> GetNavItems(SupportServiceResourceKey key, string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var resource = FindResource(key);
            var manifest = ManifestFromResource(resource);

            return manifest.Resources.Where(x => x.IsNavigationItem).Select(r => new NavItem
            {
                Title = r.ResourceTitle,
                Key = r.ResourceKey,
                Href = $"/resource?key={r.ResourceKey}&id={id}"
            });
        }

        public SiteManifest ManifestFromResource(SiteResource resource)
        {
            foreach (var manifest in this)
            {
                if (manifest.Resources.Any(item => item.ResourceKey == resource.ResourceKey))
                {
                    return manifest;
                }
            }
            return null;
        }

        public SiteResource FindResource(SupportServiceResourceKey key)
        {
            foreach (var manifest in this)
            {
                foreach (var item in manifest.Resources)
                {
                    if (item.ResourceKey == key)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}