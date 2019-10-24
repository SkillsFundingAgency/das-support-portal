using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.Support.Indexer.Jobs.DependencyResolution;
using SFA.DAS.Support.Indexer.Jobs.ScheduledJobs;

namespace SFA.DAS.Support.Indexer.Jobs
{
    public class Program
    {
        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            using (var container = IoC.Initialize())
            {
                var config = new JobHostConfiguration { JobActivator = new StructureMapJobActivator(container) };
                var isDevelopment = container.GetInstance<IEnvironmentService>().IsCurrent(DasEnv.LOCAL);

                if (isDevelopment)
                {
                    config.UseDevelopmentSettings();
                }

                config.LoggerFactory = container.GetInstance<ILoggerFactory>();

                config.UseTimers();

                var jobHost = new JobHost(config);

                jobHost.RunAndBlock();
            }
        }
    }
}
