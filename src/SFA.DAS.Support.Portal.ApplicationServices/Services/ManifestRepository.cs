using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public class ManifestRepository : IManifestRepository
    {
        private readonly IFormMapper _formMapper;
        private readonly ILog _log;
        private readonly IServiceConfiguration _serviceConfiguration;
        private readonly ISiteConnector _siteConnector;
        private readonly List<SubSiteConnectorConfig> _sites;

        public ManifestRepository(ISubSiteConnectorSettings settings,
            ISiteConnector siteConnector,
            IFormMapper formMapper,
            ILog log,
            IServiceConfiguration serviceConfiguration
        )
        {
            _siteConnector = siteConnector;
            _formMapper = formMapper;
            _log = log;
            _serviceConfiguration = serviceConfiguration;
            _sites = new List<SubSiteConnectorConfig>();

            foreach (var subSite in settings.SubSiteConnectorSettings)
            {
                _sites.Add(new SubSiteConnectorConfig
                {
                    BaseUrl = subSite.BaseUrl,
                    IdentifierUri = subSite.IdentifierUri,
                    Key = subSite.Key,
                });
            }
        }

        public async Task<ResourceResultModel> GenerateHeader(SupportServiceResourceKey key, string id)
        {
            if (!_serviceConfiguration.ResourceExists(key))
                return new ResourceResultModel { StatusCode = HttpStatusCode.NotFound };
            var resource = _serviceConfiguration.GetResource(key);
            var headerKey = resource.HeaderKey ?? key;

            var headerResource = _serviceConfiguration.GetResource(headerKey);

            var subSiteConfig = _serviceConfiguration.FindSiteConfigForManfiestElement(_sites, headerKey);

            var uri = new Uri(new Uri(subSiteConfig.BaseUrl), headerResource.ResourceUrlFormat);

            var url = string.Format(uri.ToString(), id);
            return await GetPage(url, subSiteConfig.IdentifierUri);
        }

        public async Task<string> GetChallengeForm(SupportServiceResourceKey resourceKey, SupportServiceResourceKey challengeKey, string id, string url)
        {
            var challenge = _serviceConfiguration.GetChallenge(challengeKey);
            if (challenge == null)
            {
                var exception = new ApplicationException($"The challenge {challengeKey} could not be found in any manifest");
                _log.Error(exception, $"The Manifest data is misconfigured because a Challenge was identified but not configured, please review the Manifest configuration and update it accordingly.");
                throw exception;
            }

            var subSiteConfig = _serviceConfiguration.FindSiteConfigForManfiestElement(_sites, challengeKey);

            var challengeUrl = string.Format(new Uri(new Uri(subSiteConfig.BaseUrl), challenge.ChallengeUrlFormat).ToString(), id);

            var page = await GetPage(challengeUrl, subSiteConfig.IdentifierUri);

            return _formMapper.UpdateForm(resourceKey, challengeKey, id, url, page.Resource);
        }

        public async Task<ChallengeResult> SubmitChallenge(string id, IDictionary<string, string> formData)
        {
            var redirect = formData["redirect"];
            var innerAction = formData["innerAction"];

            var challengeKey = (SupportServiceResourceKey)Enum.Parse(typeof(SupportServiceResourceKey), formData["challengeKey"]);
            var resourceKey = (SupportServiceResourceKey)Enum.Parse(typeof(SupportServiceResourceKey), formData["resourceKey"]);

            if (!_serviceConfiguration.ChallengeExists(challengeKey))
            {
                throw new MissingMemberException();
            }

            var subSiteConfig = _serviceConfiguration.FindSiteConfigForManfiestElement(_sites, challengeKey);
            var siteUri = new Uri(subSiteConfig.BaseUrl);

            var uri = new Uri(siteUri, innerAction);
            formData.Remove("redirect");
            formData.Remove("innerAction");
            formData.Remove("resourceKey");

            var html = await _siteConnector.Upload<string>(uri, formData, subSiteConfig.IdentifierUri);

            if (_siteConnector.LastException != null)
            {
                throw _siteConnector.LastException;
            }

            return HandleChallengeResponseContent(html, resourceKey, challengeKey, id, redirect);
        }

        public async Task<ResourceResultModel> SubmitApprenticeSearchRequest(SupportServiceResourceKey key,
            string hashedAccountId,
            ApprenticeshipSearchType searchType, string searchTerm)
        {
            var resource = _serviceConfiguration.GetResource(key);

            var subSiteConfig = _serviceConfiguration.FindSiteConfigForManfiestElement(_sites, key);
            var siteUri = new Uri(subSiteConfig.BaseUrl);

            var resourceSearchItemsUrl = resource.SearchItemsUrl;

            resourceSearchItemsUrl = resourceSearchItemsUrl
                .Replace("{0}", hashedAccountId)
                .Replace("{1}", $"{searchType}")
                .Replace("{2}", HttpUtility.HtmlEncode(searchTerm));

            var searchUri = new Uri(siteUri, resourceSearchItemsUrl);

            var result = new ResourceResultModel
            {
                Resource = await _siteConnector.Upload<string>(searchUri, string.Empty, subSiteConfig.IdentifierUri),
                StatusCode = _siteConnector.LastCode,
                Exception = _siteConnector.LastException
            };

            return result;
        }

        public async Task<ResourceResultModel> SubmitCreateInvitationRequest(string hashedAccountId, string email, string fullName, string supportUserEmail, string role)
        {
            const SupportServiceResourceKey key = SupportServiceResourceKey.EmployerAccountInvitationPost;

            var resource = _serviceConfiguration.GetResource(key);

            var subSiteConfig = _serviceConfiguration.FindSiteConfigForManfiestElement(_sites, key);
            var siteUri = new Uri(subSiteConfig.BaseUrl);

            var resourceUrl = resource.ResourceUrlFormat;

            resourceUrl = resourceUrl.Replace("{0}", hashedAccountId);

            var uri = new Uri(siteUri, resourceUrl);

            var request = new CreateInvitationRequest
            {
                SupportUserEmail = supportUserEmail,
                EmailOfPersonBeingInvited = email,
                NameOfPersonBeingInvited = fullName,
                RoleOfPersonBeingInvited = role
            };

            var result = new ResourceResultModel
            {
                Resource = await _siteConnector.Upload<string>(uri, JsonConvert.SerializeObject(request), subSiteConfig.IdentifierUri, isJsonContent: true),
                StatusCode = _siteConnector.LastCode,
                Exception = _siteConnector.LastException
            };

            _log.Warn($"SubmitCreateInvitationRequest result: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        public async Task SubmitChangeRoleRequest(string hashedAccountId, string userRef, string role, string supportUserEmail)
        {
            const SupportServiceResourceKey key = SupportServiceResourceKey.EmployerAccountChangeRole;

            var resource = _serviceConfiguration.GetResource(key);

            var subSiteConfig = _serviceConfiguration.FindSiteConfigForManfiestElement(_sites, key);
            var siteUri = new Uri(subSiteConfig.BaseUrl);

            var resourceUrl = resource.ResourceUrlFormat;

            resourceUrl = resourceUrl
                .Replace("{0}", hashedAccountId)
                .Replace("{1}", userRef);

            var uri = new Uri(siteUri, resourceUrl);

            var request = new UpdateRoleRequest
            {
                Role = role,
                SupportUserEmail = supportUserEmail
            };

            await _siteConnector.Upload(uri, JsonConvert.SerializeObject(request), subSiteConfig.IdentifierUri);

            if (_siteConnector.LastException != null)
            {
                throw _siteConnector.LastException;
            }
        }

        public async Task<NavViewModel> GetNav(SupportServiceResourceKey key, string id)
        {
            var navViewModel = new NavViewModel
            {
                Current = key,
                Items = _serviceConfiguration.GetNavItems(key, id).ToArray()
            };
            return await Task.FromResult(navViewModel);
        }

        public async Task<ResourceResultModel> GetResourcePage(SupportServiceResourceKey key, string id, string childId, string supportUserEmail)
        {
            var resource = _serviceConfiguration.GetResource(key);

            if (resource == null)
            {
                var exception = new ManifestRepositoryException($"The requested resource {key} was not found");
                _log.Error(exception, "A manifest was identified but not found, please review the Manifest configuration and update it accordingly.");
                throw exception;
            }

            if (!_sites.Any(x => x.Key.Equals(resource.ServiceIdentity.ToString(), StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ManifestRepositoryException($"The site {resource.ServiceIdentity} could not be found in any of the site configurations");
            }

            var site = _sites.FirstOrDefault(x => x.Key.Equals(resource.ServiceIdentity.ToString(), StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrWhiteSpace(site.BaseUrl))
            {
                throw new ManifestRepositoryException($"The site {resource.ServiceIdentity} Uri is null, Please define a BaseUrl for this Service Identity");
            }

            resource.ResourceUrlFormat = new Uri(new Uri(site.BaseUrl), resource.ResourceUrlFormat).ToString();

            var url = string.Format(resource.ResourceUrlFormat, id, WebUtility.UrlEncode(childId));

            if (resource.IncludeSupportEmail)
            {
                url = $"{url}&sid={WebUtility.UrlEncode(supportUserEmail)}";
            }

            return await GetPage(url, site.IdentifierUri);
        }

        private ChallengeResult HandleChallengeResponseContent(string responseContent,
            SupportServiceResourceKey resourceKey,
            SupportServiceResourceKey challengeKey,
            string id,
            string redirect)
        {
            var challengeValidationResult = new ChallengeValidationResult();

            try
            {
                challengeValidationResult = JsonConvert.DeserializeObject<ChallengeValidationResult>(responseContent);
            }
            catch (Exception)
            {
                _log.Debug($"Not a Valid Json Challenge Result received for account Id: {id}");
            }

            if (!challengeValidationResult.IsValidResponse)
            {
                return new ChallengeResult
                {
                    Page = _formMapper.UpdateForm(resourceKey, challengeKey, id, redirect, responseContent)
                };
            }

            return new ChallengeResult { RedirectUrl = redirect };
        }

        private async Task<ResourceResultModel> GetPage(string url, string resourceIdentity)
        {
            return new ResourceResultModel
            {
                Resource = await _siteConnector.Download(new Uri(url), resourceIdentity),
                StatusCode = _siteConnector.LastCode,
                Exception = _siteConnector.LastException
            };
        }
    }
}