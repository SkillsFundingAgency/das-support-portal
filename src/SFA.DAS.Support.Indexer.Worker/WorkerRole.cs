using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Indexer.Worker.DependencyResolution;

namespace SFA.DAS.Support.Indexer.Worker
{
    [ExcludeFromCodeCoverage]
    public class WorkerRole : RoleEntryPoint
    {
        private const int SecondsToMilliSeconds = 1000;
        private const int DelayTimeInSeconds = 1800 * 1000;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private int _delayTime = 1800 * SecondsToMilliSeconds;
        
        private IIndexSearchItems _indexer;
        private ILog _logger;
        private ISiteSettings _siteSettings;


        public override void Run()
        {
            _logger.Info("ESFA.DAS.Support.Indexer.Worker is running");

            try
            {
                RunAsync(_cancellationTokenSource.Token).Wait();
            }
            finally
            {
                _runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;
            SetupApplicationInsights();
            var container = IoC.Initialize();

            _indexer = container.GetInstance<IIndexSearchItems>();
            _logger = container.GetInstance<ILog>();
            _siteSettings = container.GetInstance<ISiteSettings>();

            if (int.TryParse(_siteSettings.DelayTimeInSeconds, out var configDelayTime))
                _delayTime = configDelayTime * SecondsToMilliSeconds;

            _logger.Info($"Support.Indexer.Worker delay time in seconds: {DelayTimeInSeconds}");

            var result = base.OnStart();
            _logger.Info("ESFA.DAS.Support.Indexer.Worker has been started");

            return result;
        }

        public override void OnStop()
        {
            _logger.Info("ESFA.DAS.Support.Indexer.Worker is stopping");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            base.OnStop();

            _logger.Info("ESFA.DAS.Support.Indexer.Worker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _indexer.Run();
                await Task.Delay(_delayTime, cancellationToken);
            }
        }

        private static void SetupApplicationInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new ApplicationInsightsInitializer());
        }
    }
}
