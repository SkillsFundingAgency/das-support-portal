using SFA.DAS.Support.Shared.SearchIndexModel;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public class CompositIndexResourceProcessor : IIndexResourceProcessor
    {
        private readonly IIndexResourceProcessor[] _indexResourceProcessors;

        public CompositIndexResourceProcessor(IIndexResourceProcessor[] indexResourceProcessors)
        {
            _indexResourceProcessors = indexResourceProcessors ?? throw new ArgumentNullException("IndexResourceProcessors");
        }

        public async Task ProcessResource(Uri uri, SearchCategory searchCategory)
        {
            foreach (var indexResourceProcessor in _indexResourceProcessors)
            {
                await indexResourceProcessor.ProcessResource(uri, searchCategory);
            }
        }
    }
}
