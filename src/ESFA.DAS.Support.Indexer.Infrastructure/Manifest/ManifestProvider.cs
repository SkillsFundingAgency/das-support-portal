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
        public IEnumerable<SearchItem> GetSearchItems(Uri collectionUri)
        {
            using (var client = new HttpClient())
            {
                var task = client.DownloadAs<IEnumerable<SearchItem>>(collectionUri);
                Task.WaitAll(task);

                return task.Result;
            }
        }

        public SiteManifest GetSiteManifest(Uri siteUri)
        {
            using (var client = new HttpClient())
            {
                var task = client.DownloadAs<SiteManifest>(new Uri(siteUri, "/api/manifest"));
                Task.WaitAll(task);

                return task.Result;
            }
        }
    }
}