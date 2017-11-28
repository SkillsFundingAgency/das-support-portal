using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Shared;
using NavItem = SFA.DAS.Support.Portal.ApplicationServices.Models.NavItem;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public class ManifestRepository : IManifestRepository
    {
        private IDownload _downloader;
        private readonly IFormMapper _formMapper;
        private readonly ILog _log;
        private readonly ISiteSettings _settings;
        private ICollection<SiteManifest> _manifests;
        private IDictionary<string, SiteResource> _resources;
        private Dictionary<string, SiteChallenge> _challanges;

        public ManifestRepository(ISiteSettings settings, IDownload downloader, IFormMapper formMapper, ILog log)
        {
            _downloader = downloader;
            _formMapper = formMapper;
            _log = log;
            _settings = settings;
        }

        public ICollection<SiteManifest> Manifests
        {
            get { return _manifests ?? (_manifests = LoadManifest()); }
        }

        public IDictionary<string, SiteResource> Resources
        {
            get { return _resources ?? (_resources = Manifests.SelectMany(x => x.Resources).ToDictionary(x => FormatKey(x.ResourceKey), x => x)); }
        }

        public IDictionary<string, SiteChallenge> Challenges
        {
            get { return _challanges ?? (_challanges = Manifests.SelectMany(x => x.Challenges).ToDictionary(x => x.ChallengeKey, x => x)); }
        }

        public bool ChallengeExists(string key)
        {
            return Challenges.ContainsKey(FormatKey(key));
        }

        public SiteChallenge GetChallenge(string key)
        {
            var challenge = Challenges[FormatKey(key)];
            var site = Manifests.FirstOrDefault(x => x.Challenges.Select(y => FormatKey(y.ChallengeKey)).Contains(key.ToLower()));
            if (site == null || site.BaseUrl == null)
            {
                throw new NullReferenceException($"The challenge {FormatKey(key)} could not be found in any manifest");
            }

            challenge.ChallengeUrlFormat = new Uri(new Uri(site.BaseUrl), challenge.ChallengeUrlFormat).ToString();
            return challenge;
        }

        public bool ResourceExists(string key)
        {
            return Resources.ContainsKey(FormatKey(key));
        }

        public object GenerateHeader(string key, string id)
        {
            var headerKey = key.ToLower().Split('/')[0] + "/header";
            if (!ResourceExists(headerKey))
            {
                return "";
            }

            var resource = GetResource(headerKey);
            var url = string.Format(resource.ResourceUrlFormat, id);
            return GetPage(url);
        }

        public string GetChallengeForm(string key, string id, string url)
        {
            var challenge = GetChallenge(key);
            var challengeUrl = string.Format(challenge.ChallengeUrlFormat, id);
            var page = GetPage(challengeUrl);
            return _formMapper.UpdateForm(key, id, url, page);
        }

        public ChallengeResult SubmitChallenge(string id, IDictionary<string, string> formData)
        {
            var redirect = formData["redirect"];
            var innerAction = formData["innerAction"];
            var challengekey = "challengeKey";
            var key = formData[challengekey];

            if (!ChallengeExists(key))
            {
                throw new MissingMemberException();
            }

            var site = FindSiteForChallenge(key);

            var uri = new Uri(site, innerAction);
            formData.Remove("redirect");
            formData.Remove("innerAction");
            formData.Remove(challengekey);


            var task = _downloader.Post(uri, formData);
            Task.WaitAll(task);

            if ((int) task.Result.StatusCode < 300)
            {
                return new ChallengeResult {RedirectUrl = redirect};
            }

            var contentTask = task.Result.Content.ReadAsStringAsync();
            Task.WaitAll(contentTask);

            var html = contentTask.Result;

            return new ChallengeResult
            {
                Page = _formMapper.UpdateForm(key, id, redirect, html)
            };
        }

        private Uri FindSiteForChallenge(string key)
        {
            var site = _manifests.FirstOrDefault(x =>
                x.Challenges.Select(y => y.ChallengeKey.ToLower()).Contains(key.ToLower()));
            return new Uri(site?.BaseUrl);
        }

        public SiteResource GetResource(string key)
        {
            var resource = Resources[FormatKey(key)];
            var site = Manifests.FirstOrDefault(x => x.Resources.Select(y => FormatKey(y.ResourceKey)).Contains(FormatKey(key)));
            if (site == null || site.BaseUrl == null)
            {
                throw new NullReferenceException($"The resource {FormatKey(key)} could not be found in any manifest");
            }
            resource.ResourceUrlFormat = new Uri(new Uri(site.BaseUrl), resource.ResourceUrlFormat).ToString();
            return resource;
        }

        public NavViewModel GetNav(string key, string id)
        {
            return new NavViewModel
            {
                Current = key,
                Items = GetNavItems(key, id).ToArray()
            };
        }

        private IEnumerable<NavItem> GetNavItems(string key, string id)
        {
            return Resources
                .Where(x => x.Key.StartsWith(key?.Split('/').FirstOrDefault()))
                .Where(x => !string.IsNullOrEmpty(x.Value.ResourceTitle))
                .Select(x =>
                    new NavItem
                    {
                        Title = x.Value.ResourceTitle,
                        Key = x.Key,
                        Href = $"/resource?key={x.Key}&id={id}"
                    });
        }

        public string GetResourcePage(string key, string id)
        {
            var resource = GetResource(key);
            var url = string.Format(resource.ResourceUrlFormat, id);
            return GetPage(url);
        }

        private string GetPage(string url)
        {
            try
            {
                var task = _downloader.Download(AddQueryString(url));
                Task.WaitAll(task);
                return task.Result;
            }
            catch 
            {
                return @"<h3 style='color:red'>There was a problem downloading this asset</h3>
<div style='display:none'>{url}</div>";
            }
        }

        private static string AddQueryString(string url)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["parent"] = string.Empty;
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        private static string FormatKey(string key)
        {
            return key?.ToLower();
        }

        private ICollection<SiteManifest> LoadManifest()
        {
            var tasks = GetManifests();
            foreach (var task in tasks)
            {
                try
                {
                    task.Value.Wait();
                }
                catch (Exception ex)
                {
                    _log.Error(ex, $"problem downloading manifest {task.Key}");
                    continue;
                }

                _log.Info($"Downloaded Manifest successfully {task.Key}");
            }

            return tasks.Values.Where(x => x.Status == TaskStatus.RanToCompletion).Select(x => x.Result).Where(x => x != null).ToList();
        }

        public IDictionary<string, Task<SiteManifest>> GetManifests()
        {
            var list = new Dictionary<string,Task<SiteManifest>>();
            foreach (var site in _settings.Sites.Where(x => !string.IsNullOrEmpty(x)))
            {
                _log.Debug($"Downloading '{site}'");
                var uri = new Uri(new Uri(site), "/api/manifest");
                var manifest = _downloader.Download<SiteManifest>(uri);
                list.Add(uri.ToString(), manifest);
            }

            return list;
        }
    }
}