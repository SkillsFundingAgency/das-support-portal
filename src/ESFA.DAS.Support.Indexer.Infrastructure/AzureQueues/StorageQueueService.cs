using ESFA.DAS.Support.Indexer.Core.Services;

namespace ESFA.DAS.Support.Indexer.Infrastructure.AzureQueues
{
    public class StorageQueueService : ITrigger
    {
        public bool HasATriggerToRun()
        {
            return false;
        }
    }
}