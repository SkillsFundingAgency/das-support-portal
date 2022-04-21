﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
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
        private readonly Dictionary<SupportServiceIdentity, Uri> _sites;

        public ManifestRepository(ISiteSettings settings,
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

            _sites = new Dictionary<SupportServiceIdentity, Uri>();

            foreach (var item in (settings.BaseUrls ?? string.Empty).Split(new[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                var subItems = item.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                var key = (SupportServiceIdentity)Enum.Parse(typeof(SupportServiceIdentity), subItems[0]);
                var value = new Uri(subItems[1]);
                _sites.Add(key, value);
            }
        }


        public async Task<ResourceResultModel> GenerateHeader(SupportServiceResourceKey key, string id)
        {
            if (!_serviceConfiguration.ResourceExists(key))
                return new ResourceResultModel { StatusCode = HttpStatusCode.NotFound };
            var resource = _serviceConfiguration.GetResource(key);
            var headerKey = resource.HeaderKey ?? key;

            var headerResource = _serviceConfiguration.GetResource(headerKey);

            var uri = new Uri(_serviceConfiguration.FindSiteBaseUriForManfiestElement(_sites, headerKey),
                                headerResource.ResourceUrlFormat);

            var url = string.Format(uri.ToString(), id);
            return await GetPage(url, key);
        }


        public async Task<string> GetChallengeForm(SupportServiceResourceKey resourceKey,
            SupportServiceResourceKey challengeKey, string id, string url)
        {
            var challenge = _serviceConfiguration.GetChallenge(challengeKey);
            if (challenge == null)
            {
                var e = new NullReferenceException($"The challenge {challengeKey} could not be found in any manifest");
                _log.Error(e,
                    $"The Manifest data is misconfigured because a Challenge was identified but not configured, please review the Manifest configuration and update it accordingly.");
                throw e;
            }

            var challengeUrl =
                string.Format(
                    new Uri(_serviceConfiguration.FindSiteBaseUriForManfiestElement(_sites, challengeKey), challenge.ChallengeUrlFormat).ToString(), id);
            var page = await GetPage(challengeUrl, resourceKey);
            return _formMapper.UpdateForm(resourceKey, challengeKey, id, url, page.Resource);
        }

        public async Task<ChallengeResult> SubmitChallenge(string id, IDictionary<string, string> formData)
        {
            var redirect = formData["redirect"];
            var innerAction = formData["innerAction"];

            var challengeKey =
                (SupportServiceResourceKey)Enum.Parse(typeof(SupportServiceResourceKey), formData["challengeKey"]);
            var resourceKey =
                (SupportServiceResourceKey)Enum.Parse(typeof(SupportServiceResourceKey), formData["resourceKey"]);

            if (!_serviceConfiguration.ChallengeExists(challengeKey)) throw new MissingMemberException();

            var siteUri = _serviceConfiguration.FindSiteBaseUriForManfiestElement(_sites, challengeKey);
            var uri = new Uri(siteUri, innerAction);
            formData.Remove("redirect");
            formData.Remove("innerAction");
            formData.Remove("resourceKey");

            var html = await _siteConnector.Upload<string>(uri, formData);

            if (_siteConnector.LastException != null) throw _siteConnector.LastException;

            return HandleChallenegeResponseContent(html, resourceKey, challengeKey, id, redirect);
        }

        public async Task<ResourceResultModel> SubmitApprenticeSearchRequest(SupportServiceResourceKey key,
            string hashedAccountId,
            ApprenticeshipSearchType searchType, string searchTerm)
        {
            var resource = _serviceConfiguration.GetResource(key);
            var siteUri = _serviceConfiguration.FindSiteBaseUriForManfiestElement(_sites, key);
            var resourceSearchItemsUrl = resource.SearchItemsUrl;

            resourceSearchItemsUrl = resourceSearchItemsUrl
                                        .Replace("{0}", hashedAccountId)
                                        .Replace("{1}", $"{searchType}")
                                        .Replace("{2}", HttpUtility.HtmlEncode(searchTerm));

            var searchUri = new Uri(siteUri, resourceSearchItemsUrl);

            ResourceResultModel result = new ResourceResultModel
            {
                //SupportServiceResourceKey = key,
                Resource = await _siteConnector.Upload<string>(searchUri, string.Empty, key),
                StatusCode = _siteConnector.LastCode,
                Exception = _siteConnector.LastException
            };


            return result;
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

        public async Task<ResourceResultModel> GetResourcePage(SupportServiceResourceKey key, string id, string childId)
        {
            var resource = _serviceConfiguration.GetResource(key);
            if (resource == null)
            {
                var e = new NullReferenceException($"The requested resource {key} was not found");
                _log.Error(e, $"A manifest was identified but not found, please review the Manifest configuration and update it accordingly.");
                throw e;
            }
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            if (!_sites.ContainsKey(resource.ServiceIdentity))
                throw new NullReferenceException(
                    $"The site {resource.ServiceIdentity} could not be found in any of the site configurations");
            var site = _sites.First(x => x.Key == resource.ServiceIdentity);

            if (site.Value == null)
                throw new NullReferenceException(
                    $"The site {resource.ServiceIdentity} Uri is null, Please define a BaseUrl for this Service Identity");


            resource.ResourceUrlFormat = new Uri(site.Value, resource.ResourceUrlFormat).ToString();


            var url = string.Format(resource.ResourceUrlFormat, id, childId);
            return await GetPage(url, key);
        }

        private ChallengeResult HandleChallenegeResponseContent(string responseContent,
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


        private async Task<ResourceResultModel> GetPage(string url, SupportServiceResourceKey supportServiceResourceKey)
        {
            var result = new ResourceResultModel();
            /// var queryString = AddQueryString(url);
            result.Resource = await _siteConnector.Download(new Uri(url), supportServiceResourceKey);
            result.StatusCode = _siteConnector.LastCode;
            result.Exception = _siteConnector.LastException;
            result.SupportServiceResourceKey = supportServiceResourceKey; //TODO : change to generic
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
    }
}