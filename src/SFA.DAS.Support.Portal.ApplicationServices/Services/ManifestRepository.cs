using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
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
        private List<SiteManifest> _manifests;
        private List<Uri> _sites;

        public ManifestRepository(ISiteSettings settings,
            ISiteConnector siteConnector,
            IFormMapper formMapper,
            ILog log,
            List<SiteManifest> manifests, Dictionary<string, SiteResource> resources,
            Dictionary<string, SiteChallenge> challenges
        )
        {
            _siteConnector = siteConnector;
            _formMapper = formMapper;
            _log = log;
            _settings = settings;
            _manifests = manifests;
            Resources = resources;
            Challenges = challenges;
           
            _sites = (_settings.BaseUrls??string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x)).Select(x=>new Uri(x)).ToList();
        }

        private IDictionary<string, SiteResource> Resources { get; set; }

        private IDictionary<string, SiteChallenge> Challenges { get; set; }

        public async Task<bool> ChallengeExists(string key)
        {
            await PollSites();
            return await Task.FromResult(Challenges.ContainsKey(FormatKey(key)));
        }

        public async Task<SiteChallenge> GetChallenge(string key)
        {
            await PollSites();
            var challenge = Challenges[FormatKey(key)];
            var siteOfChallenge = _manifests.FirstOrDefault(x =>
                x.Challenges != null &&
                x.Challenges
                    .Select(y => FormatKey(y.ChallengeKey))
                    .Contains(key.ToLower()));
            if (siteOfChallenge?.BaseUrl == null)
                throw new NullReferenceException(
                    $"The challenge {FormatKey(key)} could not be found in any manifest"
                );

            challenge.ChallengeUrlFormat = new Uri(new Uri(siteOfChallenge.BaseUrl), challenge.ChallengeUrlFormat).ToString();
            return await Task.FromResult(challenge);
        }

        public async Task<bool> ResourceExists(string key)
        {
            await PollSites();
            return await Task.FromResult(Resources.ContainsKey(FormatKey(key)));
        }

        public async Task<ResourceResultModel> GenerateHeader(string key, string id)
        {
            var headerKey = key.ToLower().Split('/')[0] + "/header";
            if (!await ResourceExists(headerKey)) return new ResourceResultModel(){ StatusCode = HttpStatusCode.NotFound};

            var resource = await GetResource(headerKey);
            var url = string.Format(resource.ResourceUrlFormat, id);
            return await GetPage(url);
        }

        public async Task<string> GetChallengeForm(string key, string id, string url)
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
            var key = formData[challengekey];

            if (!await ChallengeExists(key)) throw new MissingMemberException();

            var site = FindSiteForChallenge(key);
            var uri = new Uri(site, innerAction);
            formData.Remove("redirect");
            formData.Remove("innerAction");
            formData.Remove(challengekey);

            var html = await _siteConnector.Upload<string>(uri, formData);

            if (string.IsNullOrWhiteSpace(html))
                return new ChallengeResult { RedirectUrl = redirect };

            return new ChallengeResult
            {
                Page = _formMapper.UpdateForm(key, id, redirect, html)
            };
        }

        public async Task<List<SiteManifest>> GetManifests()
        {
            await PollSites();
            return _manifests;
        }

        public async Task<SiteResource> GetResource(string key)
        {
            await PollSites();
            var resource = Resources[FormatKey(key)];
            var site = _manifests.FirstOrDefault(x => x.Resources != null &&
                x.Resources.Select(y => FormatKey(y.ResourceKey)).Contains(FormatKey(key)));
            if (site == null || site.BaseUrl == null)
                throw new NullReferenceException($"The resource {FormatKey(key)} could not be found in any manifest");
            resource.ResourceUrlFormat = new Uri(new Uri(site.BaseUrl), resource.ResourceUrlFormat).ToString();
            return await Task.FromResult(resource);
        }

        public async Task<NavViewModel> GetNav(string key, string id)
        {
            await PollSites();
            var navViewModel = new NavViewModel
            {
                Current = key,
                Items = GetNavItems(key, id).ToArray()
            };

            return navViewModel;
        }

        public async Task<ResourceResultModel> GetResourcePage(string key, string id)
        {
            var resource = await GetResource(key);
            var url = string.Format(resource.ResourceUrlFormat, id);
            return await GetPage(url);
        }

        private async Task PollSites()
        {
            if ((!_manifests.Any())
                ||
                (_manifests.Count < _sites.Count))
            {
                _manifests = await LoadManifest();
            }
            ProcessManifests();
        }

        private void ProcessManifests()
        {
            Resources = new Dictionary<string, SiteResource>();
            Challenges = new Dictionary<string, SiteChallenge>();
            foreach (var siteManifest in _manifests)
            {
                foreach (var item in siteManifest.Resources ?? new List<SiteResource>())
                    Resources.Add(item.ResourceKey, item);
                foreach (var item in siteManifest.Challenges ?? new List<SiteChallenge>())
                    Challenges.Add(item.ChallengeKey, item);
            }
        }

        private Uri FindSiteForChallenge(string key)
        {
            var lowerCasedKey = key.ToLower();

            var siteManifest = _manifests
                .First(x => x.Challenges != null &&
                            x.Challenges.Select(y => y.ChallengeKey.ToLower())
                    .Contains(lowerCasedKey)
                );

            // DEV NOTES:  ASCS-87 - The design requries that all challenge keys are unique across all sites.
            // Its possible for two or more sites to have same challenge key by accident. 
            // Then this may return wrong one.
            return new Uri(siteManifest.BaseUrl);
        }

        private IEnumerable<NavItem> GetNavItems(string key, string id)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (id == null) throw new ArgumentNullException(nameof(id));

            return Resources
                .Where(TheResourceKeyStartsWithElementOf(key))
                .Where(ThereIsAResourceTitle())
                .Select(ANewNavItemFor(id));
        }

        private static Func<KeyValuePair<string, SiteResource>, bool> ThereIsAResourceTitle()
        {
            return x => !string.IsNullOrEmpty(x.Value.ResourceTitle);
        }

        private static Func<KeyValuePair<string, SiteResource>, bool> TheResourceKeyStartsWithElementOf(string key)
        {
            return x => x.Key.StartsWith(key.Split('/').FirstOrDefault() ?? "");
        }

        private static Func<KeyValuePair<string, SiteResource>, NavItem> ANewNavItemFor(string id)
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
            var result = new ResourceResultModel() { };
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

        private string FormatKey(string key)
        {
            return key?.ToLower();
        }

        private async Task<List<SiteManifest>> LoadManifest()
        {
            var list = new Dictionary<string, SiteManifest>();
            foreach (var site in _sites)
            {
                var uri = new Uri(site, "api/manifest");
                _log.Debug($"Downloading '{uri}'");
                try
                {
                    var manifest = await _siteConnector.Download<SiteManifest>(uri);
                    if (manifest != null )
                    {
                        list.Add(uri.ToString(), manifest);    
                    }
                    
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