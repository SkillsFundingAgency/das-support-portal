using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;

namespace SFA.DAS.Support.Indexer.Jobs.ScheduledJobs
{
    public class ImportSearchItemsJob
    {
        private readonly IIndexSearchItems _indexer;

        public ImportSearchItemsJob(IIndexSearchItems indexer)
        {
            _indexer = indexer;
        }

        public Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo timer, ILogger logger)
        {
            return _indexer.Run();
        }
    }
}
