using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DAS.Support.Indexer.ApplicationServices.Settings;
using ESFA.DAS.Support.Shared;

namespace ESFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public class IndexerService : IIndexSearchItems
    {
        private readonly ISiteSettings _settings;
        private readonly IGetSiteManifest _siteService;
        private readonly IGetSearchItemsFromASite _downloader;
        private readonly IIndexProvider _indexProvider;

        public IndexerService(ISiteSettings settings, IGetSiteManifest siteService, IGetSearchItemsFromASite downloader, IIndexProvider indexProvider)
        {
            _settings = settings;
            _siteService = siteService;
            _downloader = downloader;
            _indexProvider = indexProvider;
        }

        public void Run()
        {
            var items = FindItems();
            _indexProvider.IndexDocuments(_settings.IndexName, items);
        }

        private IEnumerable<SearchItem> FindItems()
        {
            foreach (var setting in _settings.Sites.Where(x => !string.IsNullOrEmpty(x)))
            {
                var siteManifest = _siteService.GetSiteManifest(new Uri(setting));
                foreach (var resource in siteManifest.Resources)
                {
                    if (!string.IsNullOrEmpty(resource.SearchItemsUrl))
                    {
                        var items = _downloader.GetSearchItems(new Uri(resource.SearchItemsUrl));
                        foreach (var item in items)
                        {
                            yield return item;
                        }
                    }
                }
            }
        }
    }
}