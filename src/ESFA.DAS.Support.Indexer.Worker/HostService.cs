using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DAS.Support.Indexer.ApplicationServices.Services;
using ESFA.DAS.Support.Indexer.Core.Services;
using Topshelf;
using Topshelf.Logging;

namespace ESFA.DAS.Support.Indexer.Worker
{
    public class HostService : ServiceControl
    {
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        private readonly IIndexSearchItems _indexer;
        private readonly LogWriter _log = HostLogger.Get<HostService>();
        private readonly string _testPhrase;
        private readonly ITrigger _trigger;
        private Task _task;

        public HostService(ITrigger trigger, IIndexSearchItems indexer)
        {
            _trigger = trigger;
            _indexer = indexer;
        }

        public bool Start(HostControl hostControl)
        {
            _log.InfoFormat("Service starting up: {0}", _testPhrase);

            _task = Idle();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _cancel.Cancel();

            _task.Wait();

            return true;
        }

        private async Task Idle()
        {
            if (_cancel.Token.IsCancellationRequested)
            {
                _log.Info("Goodbye, Cruel World.");
                return;
            }

            await Task.Yield();

            if (Debugger.IsAttached || _trigger.HasATriggerToRun())
                _indexer.Run();

            if (Debugger.IsAttached)
                _cancel.Cancel();

            await Task.Delay(1000);
            await Idle();
        }
    }
}