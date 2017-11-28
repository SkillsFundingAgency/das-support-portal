using SFA.DAS.Support.Indexer.Core.Services;

namespace SFA.DAS.Support.Indexer.Infrastructure.AzureQueues
{
    public class StorageQueueService : ITrigger
    {
        public bool HasATriggerToRun()
        {
            return false;
        }
    }
}