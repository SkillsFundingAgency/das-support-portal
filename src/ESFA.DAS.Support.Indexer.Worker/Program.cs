using ESFA.DAS.Support.Indexer.Worker.DependencyResolution;
using Topshelf;
using Topshelf.HostConfigurators;

namespace ESFA.DAS.Support.Indexer.Worker
{
    public class Program : TopshelfRoleEntryPoint
    {
        protected override void Configure(HostConfigurator hostConfigurator)
        {
            var container = IoC.Initialize();
            hostConfigurator.Service(settings => container.GetInstance<HostService>(), x =>
            {
                //x.BeforeStartingService(context => _log.Info("Before starting service!!"));
                //x.AfterStoppingService(context => _log.Info("After stopping service!!"));
            });
        }

        private static int Main()
        {
            return (int) HostFactory.Run(new Program().Configure);
        }
    }
}