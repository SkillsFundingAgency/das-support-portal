using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Indexer.Infrastructure.Manifest
{
    public class ManifestProvider : IGetSearchItemsFromASite, IGetSiteManifest
    {
        private readonly ISiteConnector _siteConnector;

        public ManifestProvider(ISiteConnector siteConnector)
        {
            _siteConnector = siteConnector;
        }

        public async Task<IEnumerable<T>> GetSearchItems<T>(Uri collectionUri)
        {
            var response =  await _siteConnector.Download<IEnumerable<T>>(collectionUri);

            if (_siteConnector.LastCode != HttpStatusCode.OK)
            {
                throw _siteConnector.LastException ?? new Exception($"Error while loading Search Items invalid status code {_siteConnector.LastCode}");
            }

            return response;
        }

        public async Task<SiteManifest> GetSiteManifest(Uri siteUri)
        {
            var uri = new Uri(siteUri, "/api/manifest");
            return await _siteConnector.Download<SiteManifest>(uri);
        }
    }
}