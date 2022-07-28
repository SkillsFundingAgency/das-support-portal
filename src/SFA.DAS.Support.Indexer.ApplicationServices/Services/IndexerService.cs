using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SearchIndexModel;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    [ExcludeFromCodeCoverage]
    public class IndexerService : IIndexSearchItems
    {
        private readonly ISiteConnector _dataSource;
        private readonly IIndexNameCreator _indexNameCreator;
        private readonly IIndexProvider _indexProvider;
        private readonly IIndexResourceProcessor _indexResourceProcessor;

        private readonly Stopwatch _indexTimer = new Stopwatch();
        private readonly ILog _logger;
        private readonly Stopwatch _queryTimer = new Stopwatch();
        private readonly Stopwatch _runtimer = new Stopwatch();
        private readonly ISearchSettings _searchSettings;
        private readonly ISubSiteConnectorSettings _siteSettings;
        private readonly ServiceConfiguration _manifests;

        public IndexerService(ISubSiteConnectorSettings settings,
            ISiteConnector downloader,
            IIndexProvider indexProvider,
            ISearchSettings searchSettings,
            ILog logger,
            IIndexNameCreator indexNameCreator,
            IIndexResourceProcessor indexResourceProcessor, ServiceConfiguration manifests)
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
            _logger.Info($"Starting Indexer Service Run");

            _runtimer.Start();
            try
            {
                foreach (var subSite in _siteSettings.SubSiteConnectorSettings)
                {
                    var siteUri = new Uri(subSite.BaseUrl);
                    var siteManifest = _manifests
                        .FirstOrDefault(x => x.Resources.Any(y => y.ServiceIdentity.ToString().Equals(subSite.Key, StringComparison.InvariantCultureIgnoreCase)));

                    _queryTimer.Stop();

                    if (string.IsNullOrWhiteSpace(subSite.BaseUrl))
                    {
                        _logger.Info($"Site Manifest: at Uri: {siteUri} not found or has no BaseUrl configured");
                        continue;
                    }

                    _logger.Info(
                        $"Site Manifest: Uri: {subSite.BaseUrl ?? "Missing Url"} # Challenges: {siteManifest.Challenges?.Count() ?? 0} # Resources: {siteManifest.Resources?.Count() ?? 0}");

                    var resourcesToIndex = siteManifest.Resources?.Where(x =>
                                            !string.IsNullOrWhiteSpace(x.SearchItemsUrl) &&
                                            !string.IsNullOrWhiteSpace(subSite.BaseUrl) &&
                                            x.SearchCategory != SearchCategory.None).ToList();

                    if (resourcesToIndex == null) continue;

                    foreach (var resource in resourcesToIndex)
                    {
                        _logger.Info($"Processing Resource: Key: {resource.ResourceKey} Title: {resource.ResourceTitle} SearchUri: {resource.SearchItemsUrl ?? "not set"}");

                        var baseUri = new Uri(subSite.BaseUrl);

                        await _indexResourceProcessor.ProcessResource(new IndexResourceProcessorModel
                        {
                            BasUri = baseUri,
                            SiteResource = resource,
                            ResourceIdentifier = subSite.IdentifierUri
                        });
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
                _logger.Info($"Ending Indexer Service Run Elapsed Time {_runtimer.Elapsed}");
            }
        }
    }
}