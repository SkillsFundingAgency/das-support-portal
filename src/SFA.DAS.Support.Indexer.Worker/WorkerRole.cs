using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Indexer.Worker.DependencyResolution;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Indexer.Worker
{
    [ExcludeFromCodeCoverage]
    public class WorkerRole : RoleEntryPoint
    {
        private const int SecondsToMilliSeconds = 1000;
        public static readonly List<SiteManifest> SiteManifests = new List<SiteManifest>();
        public static readonly Dictionary<string, SiteResource> SiteResources = new Dictionary<string, SiteResource>();

        public static readonly Dictionary<string, SiteChallenge> SiteChallenges =
            new Dictionary<string, SiteChallenge>();

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private int _delayTime = 1800 * SecondsToMilliSeconds;
        private IIndexSearchItems _indexer;
        private ILog _logger;
        private int _delayTimeInSeconds = 1800 * 1000;
        private ISearchSettings _searchSettings;
        private ISiteSettings _siteSettings;


        public override void Run()
        {
            _logger.Info("ESFA.DAS.Support.Indexer.Worker is running");

            try
            {
                RunAsync(cancellationTokenSource.Token).Wait();
            }
            finally
            {
                runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            var container = IoC.Initialize();

            _indexer = container.GetInstance<IIndexSearchItems>();
            _logger = container.GetInstance<ILog>();
            _siteSettings = container.GetInstance<ISiteSettings>();

            if (int.TryParse(_siteSettings.DelayTimeInSeconds, out var configDelayTime))
                _delayTime = configDelayTime * SecondsToMilliSeconds;

            _logger.Info($"Support.Indexer.Worker delay time in seconds: {_delayTimeInSeconds}");

            var result = base.OnStart();
            _logger.Info("ESFA.DAS.Support.Indexer.Worker has been started");

            return result;
        }

        public override void OnStop()
        {
            _logger.Info("ESFA.DAS.Support.Indexer.Worker is stopping");

            cancellationTokenSource.Cancel();
            runCompleteEvent.WaitOne();

            base.OnStop();

            _logger.Info("ESFA.DAS.Support.Indexer.Worker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _indexer.Run();
                await Task.Delay(_delayTime);
            }
        }
    }
}