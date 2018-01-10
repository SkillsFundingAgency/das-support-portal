using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Indexer.Worker.DependencyResolution;

namespace SFA.DAS.Support.Indexer.Worker
{
    [ExcludeFromCodeCoverage]
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private IIndexSearchItems _indexer;
        private int _delayTimeInSeconds = 1800 * 1000;


        public override void Run()
        {
            Trace.TraceInformation("ESFA.DAS.Support.Indexer.Worker is running");

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


            if (int.TryParse(CloudConfigurationManager.GetSetting("DelayTimeInSeconds"), out int configDelayTime))
            {
                _delayTimeInSeconds = configDelayTime * 1000;
            }

            var result = base.OnStart();
            Trace.TraceInformation("ESFA.DAS.Support.Indexer.Worker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("ESFA.DAS.Support.Indexer.Worker is stopping");

            cancellationTokenSource.Cancel();
            runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("ESFA.DAS.Support.Indexer.Worker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _indexer.Run();
                await Task.Delay(_delayTimeInSeconds);
            }
        }
    }
}