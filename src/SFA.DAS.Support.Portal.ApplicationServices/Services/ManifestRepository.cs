using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.Navigation;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public class ManifestRepository : IManifestRepository
    {
        private readonly IFormMapper _formMapper;
        private readonly ILog _log;
        private readonly ISiteSettings _settings;
        private readonly ISiteConnector _siteConnector;
        private SupportServiceManifests _manifests;
        private readonly Dictionary<SupportServiceIdentity, Uri> _sites;

        public ManifestRepository(ISiteSettings settings,
            ISiteConnector siteConnector,
            IFormMapper formMapper,
            ILog log,
           SupportServiceManifests manifests
        )
        {
            _siteConnector = siteConnector;
            _formMapper = formMapper;
            _log = log;
            _settings = settings;
            _manifests = manifests;
            Resources = new Dictionary<SupportServiceResourceKey, SiteResource>();
            Challenges = new Dictionary<SupportServiceResourceKey, SiteChallenge>();
            foreach (var siteManifest in _manifests)
            {
                foreach (var item in siteManifest.Value.Resources ?? new List<SiteResource>())
                    Resources.Add(item.ResourceKey, item);
                foreach (var item in siteManifest.Value.Challenges ?? new List<SiteChallenge>())
                    Challenges.Add(item.ChallengeKey, item);
            }

            _sites = new Dictionary<SupportServiceIdentity, Uri>();

            foreach (var item in (_settings.BaseUrls ?? string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                var subItems = item.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
                var key = (SupportServiceIdentity)Enum.Parse(typeof(SupportServiceIdentity), subItems[0]);
                var value =   new Uri(subItems[1]);
                _sites.Add(key,value);

            }
        }

        private IDictionary<SupportServiceResourceKey, SiteResource> Resources { get; set; }

        private IDictionary<SupportServiceResourceKey, SiteChallenge> Challenges { get; set; }

        public async Task<bool> ChallengeExists(SupportServiceResourceKey key)
        {
            return await Task.FromResult(Challenges.Keys.Contains(key));
        }

        public async Task<SiteChallenge> GetChallenge(SupportServiceResourceKey key)
        {
            var challenge = Challenges[key];

            var manifestOfChallenge = _manifests.FirstOrDefault(x =>
                x.Value.Challenges != null &&
                x.Value.Challenges.Select(y => y.ChallengeKey).Contains(key));

            var siteOfChallenge = _sites.FirstOrDefault(x => x.Key == manifestOfChallenge.Key);


            if (siteOfChallenge.Value == null)
                throw new NullReferenceException(
                    $"The challenge {FormatKey(key)} could not be found in any manifest"
                );

            challenge.ChallengeUrlFormat =
                new Uri(siteOfChallenge.Value, challenge.ChallengeUrlFormat).ToString();
            return await Task.FromResult(challenge);
        }

        public async Task<bool> ResourceExists(SupportServiceResourceKey key)
        {
            return await Task.FromResult(Resources.Keys.Contains(key));
        }

        public async Task<ResourceResultModel> GenerateHeader(SupportServiceResourceKey key, string id)
        {
            var headerKey = key;
            if (!await ResourceExists(headerKey)) return new ResourceResultModel {StatusCode = HttpStatusCode.NotFound};

            var resource = await GetResource(headerKey);
            var url = string.Format(resource.ResourceUrlFormat, id);
            return await GetPage(url);
        }

        public async Task<string> GetChallengeForm(SupportServiceResourceKey key, string id, string url)
        {
            var challenge = await GetChallenge(key);
            var challengeUrl = string.Format(challenge.ChallengeUrlFormat, id);
            var page = await GetPage(challengeUrl);
            return _formMapper.UpdateForm(key, id, url, page.Resource);
        }

        public async Task<ChallengeResult> SubmitChallenge(string id, IDictionary<string, string> formData)
        {
            var redirect = formData["redirect"];
            var innerAction = formData["innerAction"];
            var challengekey = "challengeKey";
            var key = (SupportServiceResourceKey) Enum.Parse(typeof(SupportServiceResourceKey), formData[challengekey]);

            if (!await ChallengeExists(key)) throw new MissingMemberException();

            var site = FindSiteForChallenge(key);
            var uri = new Uri(site, innerAction);
            formData.Remove("redirect");
            formData.Remove("innerAction");
            formData.Remove(challengekey);

            var html = await _siteConnector.Upload<string>(uri, formData);

            return AcceptTheHtmlOrTheForbiddenStatusCode(html, key, id, redirect);
        }

        public async Task<SupportServiceManifests> GetManifests()
        {
          
            return _manifests;
        }

        public async Task<SiteResource> GetResource(SupportServiceResourceKey key)
        {
         
            var resource = Resources[key];
            var manifest = _manifests.FirstOrDefault(x => x.Value.Resources != null &&
                                                      x.Value.Resources.Select(y => FormatKey(y.ResourceKey))
                                                          .Contains(FormatKey(key)));

            var site = _sites.FirstOrDefault(x=>x.Key == manifest.Key);

            if (site.Value == null || site.Value == null)
                throw new NullReferenceException($"The resource {FormatKey(key)} could not be found in any manifest");
            resource.ResourceUrlFormat = new Uri(site.Value, resource.ResourceUrlFormat).ToString();
            return await Task.FromResult(resource);
        }

        public async Task<NavViewModel> GetNav(SupportServiceResourceKey key, string id)
        {
            
            var navViewModel = new NavViewModel
            {
                Current = key,
                Items = GetNavItems(key, id).ToArray()
            };

            return navViewModel;
        }

        public async Task<ResourceResultModel> GetResourcePage(SupportServiceResourceKey key, string id)
        {
            var resource = await GetResource(key);
            var url = string.Format(resource.ResourceUrlFormat, id);
            return await GetPage(url);
        }

        private ChallengeResult AcceptTheHtmlOrTheForbiddenStatusCode(string html, SupportServiceResourceKey key,
            string id, string redirect)
        {
            if (string.IsNullOrWhiteSpace(html) &&
                _siteConnector.LastCode == HttpStatusCode.Forbidden)
            {
                html = _siteConnector.LastContent;
                return new ChallengeResult
                {
                    Page = _formMapper.UpdateForm(key, id, redirect, html)
                };
            }

            return new ChallengeResult {RedirectUrl = redirect};
        }

        private Uri FindSiteForChallenge(SupportServiceResourceKey key)
        {
            var siteManifest = _manifests
                .First(x => x.Value.Challenges != null &&
                            x.Value.Challenges.Select(y => y.ChallengeKey)
                                .Contains(key)
                );

            var site = _sites.FirstOrDefault(x => x.Key == siteManifest.Key);

            return site.Value;
        }

        private IEnumerable<NavItem> GetNavItems(SupportServiceResourceKey key, string id)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (id == null) throw new ArgumentNullException(nameof(id));

            return Resources
                .Where(TheResourceKeyStartsWithElementOf(key))
                .Where(ThereIsAResourceTitle())
                .Select(ANewNavItemFor(id));
        }

        private static Func<KeyValuePair<SupportServiceResourceKey, SiteResource>, bool> ThereIsAResourceTitle()
        {
            return x => !string.IsNullOrEmpty(x.Value.ResourceTitle);
        }

        private static Func<KeyValuePair<SupportServiceResourceKey, SiteResource>, bool>
            TheResourceKeyStartsWithElementOf(SupportServiceResourceKey key)
        {
            return x => x.Key == key;
        }

        private static Func<KeyValuePair<SupportServiceResourceKey, SiteResource>, NavItem> ANewNavItemFor(string id)
        {
            return x =>
                new NavItem
                {
                    Title = x.Value.ResourceTitle,
                    Key = x.Key,
                    Href = $"/resource?key={x.Key}&id={id}"
                };
        }

        private async Task<ResourceResultModel> GetPage(string url)
        {
            var result = new ResourceResultModel();
            var queryString = AddQueryString(url);
            result.Resource = await _siteConnector.Download(queryString);
            result.StatusCode = _siteConnector.LastCode;
            result.Exception = _siteConnector.LastException;
            return result;
        }

        private string AddQueryString(string url)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["parent"] = string.Empty;
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        private string FormatKey(SupportServiceResourceKey key)
        {
            return key.ToString();
        }

        private async Task<List<SiteManifest>> LoadManifest()
        {
            var list = new Dictionary<string, SiteManifest>();
            foreach (var site in _sites)
            {
                var uri = new Uri(site.Value, "api/manifest");
                _log.Debug($"Downloading '{uri}'");
                try
                {
                    var manifest = await _siteConnector.Download<SiteManifest>(uri);
                    if (manifest != null) list.Add(uri.ToString(), manifest);
                }
                catch (Exception ex)
                {
                    _log.Error(ex,
                        $"Exception occured calling {nameof(ISiteConnector)}.{nameof(ISiteConnector.Download)}<{nameof(SiteManifest)}>('{uri}'");
                }
            }

            return list.Values.ToList();
        }
    }
}