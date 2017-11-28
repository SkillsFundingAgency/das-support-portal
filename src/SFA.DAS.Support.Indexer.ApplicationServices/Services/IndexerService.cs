using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public class IndexerService : IIndexSearchItems
    {
        private readonly IGetSearchItemsFromASite _downloader;
        private readonly IIndexProvider _indexProvider;
        private readonly ISiteSettings _settings;
        private readonly IGetSiteManifest _siteService;
        private readonly Stopwatch _indexTimer = new Stopwatch();
        private readonly Stopwatch _queryTimer = new Stopwatch();

        private readonly Stopwatch _runtimer = new Stopwatch();

        public IndexerService(ISiteSettings settings, IGetSiteManifest siteService, IGetSearchItemsFromASite downloader,
            IIndexProvider indexProvider)
        {
            _settings = settings;
            _siteService = siteService;
            _downloader = downloader;
            _indexProvider = indexProvider;
        }

        public void Run()
        {
            try
            {
                CreateIndexIfNotExists();
                _runtimer.Start();
                MergeOrCreateItems().Wait();
                _indexTimer.Stop();
                _queryTimer.Stop();
                Console.WriteLine(
                    $"Indexer: Elapsed {_runtimer.Elapsed}\r\nQuery: {_queryTimer.Elapsed}\r\nIndexing {_indexTimer.Elapsed}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void CreateIndexIfNotExists()
        {
            if (!_indexProvider.IndexExists(_settings.IndexName))
                _indexProvider.CreateIndex<SearchItem>(_settings.IndexName);
        }

        private async Task MergeOrCreateItems()
        {
            try
            {
                _queryTimer.Start();
                foreach (var setting in _settings.Sites.Where(x => !string.IsNullOrEmpty(x)))
                {
                    var siteManifest = await _siteService.GetSiteManifest(new Uri(setting));
                    _queryTimer.Stop();
                    if (string.IsNullOrEmpty(siteManifest.BaseUrl)) continue;
                    Console.WriteLine(
                        $"Site Manifest: Uri: {siteManifest.BaseUrl ?? "Missing Url"} Version: {siteManifest.Version ?? "Missing Version"} # Challenges: {siteManifest.Challenges?.Count() ?? 0} # Resources: {siteManifest.Resources?.Count() ?? 0}");

                    foreach (var resource in siteManifest.Resources ?? new List<SiteResource>())
                    {
                        if (string.IsNullOrEmpty(resource.SearchItemsUrl)) continue;

                        Console.WriteLine(
                            $"Processing Resource: Key: {resource.ResourceKey} Title: {resource.ResourceTitle} SearchUri: {resource.SearchItemsUrl ?? "not set"}");
                        var baseUri = new Uri(siteManifest.BaseUrl);
                        var uri = new Uri(baseUri, resource.SearchItemsUrl);
                        _queryTimer.Start();
                        var searchItems = await _downloader.GetSearchItems(uri);
                        _queryTimer.Stop();
                        _indexTimer.Start();
                        _indexProvider.IndexDocuments(
                            _settings.IndexName,
                            searchItems);
                        _indexTimer.Stop();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}