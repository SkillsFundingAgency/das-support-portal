using System;
using System.Collections.Generic;
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
        private readonly IIndexProvider _indexProvider;
        private readonly IIndexResourceProcessor _indexResourceProcessor;

        private readonly Stopwatch _indexTimer = new Stopwatch();
        private readonly ILog _logger;
        private readonly Stopwatch _queryTimer = new Stopwatch();
        private readonly Stopwatch _runtimer = new Stopwatch();
        private readonly ISearchSettings _searchSettings;
        private readonly ISiteSettings _siteSettings;
        private readonly SupportServiceManifests _manifests;
        public IndexerService(ISiteSettings settings,
            IGetSearchItemsFromASite downloader,
            IIndexProvider indexProvider,
            ISearchSettings searchSettings,
            ILog logger,
            IIndexNameCreator indexNameCreator,
            IIndexResourceProcessor indexResourceProcessor, SupportServiceManifests manifests)
        {
            _siteSettings = settings;
            _dataSource = downloader;
            _indexProvider = indexProvider;
            _searchSettings = searchSettings;
            _logger = logger;
            _indexNameCreator = indexNameCreator;
            _indexResourceProcessor = indexResourceProcessor;
            _manifests = manifests;
        }

        public async Task Run()
        {
            _runtimer.Start();
            try
            {

                Dictionary<SupportServiceIdentity,string> subSites = new Dictionary<SupportServiceIdentity, string>();

                foreach (var subSite in _siteSettings.BaseUrls.Split(',').ToList())
                {
                    if (string.IsNullOrWhiteSpace(subSite[0].ToString())) continue;
                    if (string.IsNullOrWhiteSpace(subSite[1].ToString())) continue;
                    subSites.Add((SupportServiceIdentity)Enum.Parse(typeof(SupportServiceIdentity), subSite[0].ToString()), subSite[1].ToString());
                }
                

                foreach (var subSite in subSites)
                {
                    var siteUri = new Uri(subSite.Value);
                    var siteManifest = _manifests.FirstOrDefault(x=>x.Key == subSite.Key);


                    _queryTimer.Stop();

                    if (string.IsNullOrWhiteSpace(subSite.Value))
                    {
                        _logger.Info($"Site Manifest: at Uri: {siteUri} not found or has no BaseUrl configured");
                        continue;
                    }

                    _logger.Info(
                        $"Site Manifest: Uri: {subSite.Value ?? "Missing Url"} # Challenges: {siteManifest.Value.Challenges?.Count() ?? 0} # Resources: {siteManifest.Value.Resources?.Count() ?? 0}");

                    var resourcesToIndex = siteManifest.Value.Resources?.Where(x =>
                                            !string.IsNullOrWhiteSpace(x.SearchItemsUrl) &&
                                            !string.IsNullOrWhiteSpace(subSite.Value) &&
                                            x.SearchCategory != SearchCategory.None).ToList();

                    if (resourcesToIndex == null) continue;

                    foreach (var resource in resourcesToIndex)
                    {
                        if (resource.SearchItemsUrl == null) continue;
                        _logger.Info(
                            $"Processing Resource: Key: {resource.ResourceKey} Title: {resource.ResourceTitle} SearchUri: {resource.SearchItemsUrl ?? "not set"}");

                        var baseUri = new Uri(subSite.Value);
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