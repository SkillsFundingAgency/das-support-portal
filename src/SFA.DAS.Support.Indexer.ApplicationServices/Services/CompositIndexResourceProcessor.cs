using System;
using System.Threading.Tasks;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public class CompositIndexResourceProcessor : IIndexResourceProcessor
    {
        private readonly IIndexResourceProcessor[] _indexResourceProcessors;

        public CompositIndexResourceProcessor(IIndexResourceProcessor[] indexResourceProcessors)
        {
            _indexResourceProcessors =
                indexResourceProcessors ?? throw new ArgumentNullException("IndexResourceProcessors");
        }

        public async Task ProcessResource(IndexResourceProcessorModel resourceProcessorModel)
        {
            foreach (var indexResourceProcessor in _indexResourceProcessors)
            {
                await indexResourceProcessor.ProcessResource(resourceProcessorModel);
            }
        }
    }
}