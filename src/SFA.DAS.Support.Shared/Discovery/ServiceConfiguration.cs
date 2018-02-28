using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Navigation;

namespace SFA.DAS.Support.Shared.Discovery
{

    public class ServiceConfiguration : List<SiteManifest>, IServiceConfiguration
    {
        public Uri FindSiteBaseUriForManfiestElement(Dictionary<SupportServiceIdentity, Uri> sites,  SupportServiceResourceKey key)
        {
            if (sites == null) throw new ArgumentNullException(nameof(sites));
            SiteManifest manifest = null;
            foreach (var item in this)
            {
                if (item.Challenges.Any(c => c.ChallengeKey == key)
                 ||
                    item.Resources.Any(r => r.ResourceKey == key))
                {
                    manifest = item;
                }
            }
            if (manifest == null) throw new ArgumentNullException(nameof(manifest));
            var site = sites.FirstOrDefault(x => x.Key == manifest.ServiceIdentity);
            if (site.Value == null)  throw new ArgumentNullException(nameof(site));;
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
        public  SiteChallenge GetChallenge(SupportServiceResourceKey key)
        {
            var challenge = FindChallenge(key);
           return  challenge;
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