using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public class IndexerService : IIndexSearchItems
    {
        private const int _indexToRetain = 5;
        private readonly IGetSearchItemsFromASite _dataSource;
        private readonly IIndexNameCreator _indexNameCreator;
        private readonly IIndexResourceProcessor _indexResourceProcessor;
        private readonly IIndexProvider _indexProvider;

        private readonly Stopwatch _indexTimer = new Stopwatch();
        private readonly ILog _logger;
        private readonly Stopwatch _queryTimer = new Stopwatch();
        private readonly Stopwatch _runtimer = new Stopwatch();
        private readonly ISearchSettings _searchSettings;
        private readonly ISiteSettings _settings;
        private readonly IGetSiteManifest _siteService;

        public IndexerService(ISiteSettings settings,
            IGetSiteManifest siteService,
            IGetSearchItemsFromASite downloader,
            IIndexProvider indexProvider,
            ISearchSettings searchSettings,
            ILog logger,
            IIndexNameCreator indexNameCreator,
            IIndexResourceProcessor indexResourceProcessor)
        {
            _settings = settings;
            _siteService = siteService;
            _dataSource = downloader;
            _indexProvider = indexProvider;
            _searchSettings = searchSettings;
            _logger = logger;
            _indexNameCreator = indexNameCreator;
            _indexResourceProcessor = indexResourceProcessor;
        }

        public async Task Run()
        {
            _runtimer.Start();
            try
            {
                var subSites = _settings.BaseUrls?.Split(Convert.ToChar(","))?.Where(x => !string.IsNullOrEmpty(x));

                if(subSites == null) throw new Exception("Sub sites base Url not specified in config");
           

                foreach (var subSite in subSites)
                {
                    var siteUri = new Uri(subSite);
                    var siteManifest = await _siteService.GetSiteManifest(siteUri);


                    _queryTimer.Stop();

                    if (string.IsNullOrWhiteSpace(siteManifest?.BaseUrl))
                    {
                        _logger.Info($"Site Manifest: at Uri: {siteUri} not found or has no BaseUrl configured");
                        continue;
                    }

                    _logger.Info(
                        $"Site Manifest: Uri: {siteManifest.BaseUrl ?? "Missing Url"} Version: {siteManifest.Version ?? "Missing Version"} # Challenges: {siteManifest.Challenges?.Count() ?? 0} # Resources: {siteManifest.Resources?.Count() ?? 0}");

                    var resourcesToIndex = siteManifest.Resources?.Where(x => !string.IsNullOrWhiteSpace(x.SearchItemsUrl) &&
                                                                              !string.IsNullOrWhiteSpace(siteManifest.BaseUrl) &&
                                                                              x.SearchCategory != SearchCategory.None);

                    if (resourcesToIndex == null) continue;

                    foreach (var resource in resourcesToIndex)
                    {
                        _logger.Info( $"Processing Resource: Key: {resource.ResourceKey} Title: {resource.ResourceTitle} SearchUri: {resource.SearchItemsUrl ?? "not set"}");

                        var baseUri = new Uri(siteManifest.BaseUrl);
                        var uri = new Uri(baseUri, resource.SearchItemsUrl);

                       await _indexResourceProcessor.ProcessResource(uri, resource.SearchCategory);
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

    }
}