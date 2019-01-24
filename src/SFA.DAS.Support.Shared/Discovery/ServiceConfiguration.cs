using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Navigation;

namespace SFA.DAS.Support.Shared.Discovery
{

    public class ServiceConfiguration : List<SiteManifest>, IServiceConfiguration
    {
        public Uri FindSiteBaseUriForManfiestElement(Dictionary<SupportServiceIdentity, Uri> sites, SupportServiceResourceKey key)
        {
            if (sites == null) throw new ArgumentNullException(nameof(sites));
            SiteResource resource = null;
            KeyValuePair<SupportServiceIdentity, Uri> site;
            foreach (var item in this)
            {
                if (item.Resources.Any(c => c.ResourceKey == key))
                {
                    resource = item.Resources.First(x => x.ResourceKey == key);
                    break;
                }
            }

            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource)); ;
            }
            site = sites.FirstOrDefault(x => x.Key == resource.ServiceIdentity);
            return site.Value;
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
            var resource = this.FindResource(key);
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