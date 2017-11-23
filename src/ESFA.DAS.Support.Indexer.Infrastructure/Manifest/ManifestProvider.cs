using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ESFA.DAS.Support.Indexer.ApplicationServices.Services;
using ESFA.DAS.Support.Indexer.Infrastructure.Extensions;
using ESFA.DAS.Support.Shared;

namespace ESFA.DAS.Support.Indexer.Infrastructure.Manifest
{
    public class ManifestProvider : IGetSearchItemsFromASite, IGetSiteManifest
    {
        public async Task<IEnumerable<SearchItem>> GetSearchItems(Uri collectionUri)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 1, 0, 0);
                return await client.DownloadAs<IEnumerable<SearchItem>>(collectionUri);
            }
        }

        public async Task<SiteManifest> GetSiteManifest(Uri siteUri)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 1, 0);
                return await client.DownloadAs<SiteManifest>(new Uri(siteUri, "/api/manifest"));
            }
        }
    }
}