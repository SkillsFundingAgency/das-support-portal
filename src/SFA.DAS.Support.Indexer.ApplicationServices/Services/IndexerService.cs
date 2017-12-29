using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public class IndexerService : IIndexSearchItems
    {
        private readonly IGetSearchItemsFromASite _downloader;
        private readonly IIndexProvider _indexProvider;
        private readonly ISiteSettings _settings;
        private readonly IGetSiteManifest _siteService;
        private readonly ISearchSettings _searchSettings;
        private readonly ILog _logger;
        private readonly IIndexNameCreator _indexNameCreator;


        private readonly Stopwatch _indexTimer = new Stopwatch();
        private readonly Stopwatch _queryTimer = new Stopwatch();
        private readonly Stopwatch _runtimer = new Stopwatch();

        public IndexerService(ISiteSettings settings,
            IGetSiteManifest siteService,
            IGetSearchItemsFromASite downloader,
            IIndexProvider indexProvider,
            ISearchSettings searchSettings,
            ILog logger,
            IIndexNameCreator indexNameCreator)
        {
            _settings = settings;
            _siteService = siteService;
            _downloader = downloader;
            _indexProvider = indexProvider;
            _searchSettings = searchSettings;
            _logger = logger;
            _indexNameCreator = indexNameCreator;

        }

        public async Task Run()
        {
            _runtimer.Start();
            try
            {

                var subSites = _settings.Sites.Where(x => !string.IsNullOrEmpty(x));

                foreach (var subSite in subSites)
                {
                    var siteManifest = await _siteService.GetSiteManifest(new Uri(subSite));
                    _queryTimer.Stop();

                    _logger.Info($"Site Manifest: Uri: {siteManifest.BaseUrl ?? "Missing Url"} Version: {siteManifest.Version ?? "Missing Version"} # Challenges: {siteManifest.Challenges?.Count() ?? 0} # Resources: {siteManifest.Resources?.Count() ?? 0}");

                    var resourcesToIndex = siteManifest.Resources?.Where(x => !string.IsNullOrEmpty(x.SearchItemsUrl) &&
                                                                    !string.IsNullOrEmpty(siteManifest.BaseUrl) &&
                                                                    x.SearchCategory != SearchCategory.None);

                    if (resourcesToIndex == null) continue;

                    foreach (var resource in resourcesToIndex)
                    {
                        _logger.Info($"Processing Resource: Key: {resource.ResourceKey} Title: {resource.ResourceTitle} SearchUri: {resource.SearchItemsUrl ?? "not set"}");
                        var baseUri = new Uri(siteManifest.BaseUrl);
                        var uri = new Uri(baseUri, resource.SearchItemsUrl);


                        switch (resource.SearchCategory)
                        {
                            case SearchCategory.User:
                                await ProcessResource<UserSearchModel>(resource, uri);
                                break;
                            case SearchCategory.Account:
                                await ProcessResource<AccountSearchModel>(resource, uri);
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while running indexer");
            }
            finally
            {
                _runtimer.Stop();
                _logger.Info($"Indexer Run: Elapsed Time {_runtimer.Elapsed}");
            }
        }

        private async Task ProcessResource<T>(SiteResource resource, Uri uri) where T : class
        {
            try
            {
                var newIndexName = _indexNameCreator.CreateNewIndexName(_searchSettings.IndexNameFormat, _settings.EnvironmentName, resource.SearchCategory);
                CreateIndex<T>(newIndexName);

                _queryTimer.Start();
                _logger.Info($" Downloading Index Records ...");
                var searchItems = await _downloader.GetSearchItems<T>(uri);
                _queryTimer.Stop();

                _indexTimer.Start();
                _logger.Info($" Indexing Documents ...");
                _indexProvider.IndexDocuments<T>(newIndexName, searchItems);
                _indexTimer.Stop();

                _logger.Info($"Creating Index Alias and Swapping from old to new index ...");
                var indexAlias = _indexNameCreator.CreateIndexesAliasName(_searchSettings.IndexNameFormat, _settings.EnvironmentName, resource.SearchCategory);
                _indexProvider.CreateIndexAlias(newIndexName, indexAlias);

                DeleteIndex(newIndexName, 0, resource.SearchCategory);

                _indexTimer.Stop();
                _queryTimer.Stop();
                _logger.Info($"Query Elapse Time For {nameof(T)} : {_queryTimer.Elapsed} - Indexing Time {_indexTimer.Elapsed}");

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception while Indexing {nameof(T)}");
            }
        }

        private void CreateIndex<T>(string derivedIndexName) where T : class
        {
            _logger.Info($"Creating Index {derivedIndexName}...");

            _indexProvider.CreateIndex<T>(derivedIndexName);

            _logger.Info($" Index  {derivedIndexName} sucessfully created...");
        }

        private void DeleteIndex(string newIndexName, int fromDayInterval, SearchCategory searchCategory)
        {
            _logger.Info($"Deleting Indexes ...");
            var deleteIndexName = _indexNameCreator.CreateIndexesToDeleteName(_searchSettings.IndexNameFormat, _settings.EnvironmentName, searchCategory);
            _indexProvider.DeleteIndexes(x => x.StartsWith(deleteIndexName) && !x.Equals(newIndexName, StringComparison.OrdinalIgnoreCase));
            _logger.Info($"Deleting Old Indexes Completed...");
        }

    }
}