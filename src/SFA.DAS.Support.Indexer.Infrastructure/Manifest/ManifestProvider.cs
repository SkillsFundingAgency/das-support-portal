using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Indexer.Infrastructure.Extensions;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Indexer.Infrastructure.Manifest
{
    public class ManifestProvider : IGetSearchItemsFromASite, IGetSiteManifest
    {
        private readonly HttpClient _httpClient;
        private readonly TimeSpan _httpClientSearchItemTimeout = new TimeSpan(0, 1, 0, 0);

        public ManifestProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<SearchItem>> GetSearchItems(Uri collectionUri)
        {
            //_httpClient.Timeout = _httpClientSearchItemTimeout;
            return await _httpClient.DownloadAs<IEnumerable<SearchItem>>(collectionUri);
        }

        public async Task<SiteManifest> GetSiteManifest(Uri siteUri)
        {
            //_httpClient.Timeout = new TimeSpan(0, 0, 1, 0);
            return await _httpClient.DownloadAs<SiteManifest>(new Uri(siteUri, "/api/manifest"));
        }

    }
}