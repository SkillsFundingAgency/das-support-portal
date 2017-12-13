using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Shared;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ISearchProvider _provider;

        public EntityRepository(ISearchProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<GenericSearchResult> Search(string query)
        {
            return _provider.Search<SearchItem>(query)?.Select(x => new GenericSearchResult
                {
                    SearchResultCategory = x.SearchResultCategory,
                    SearchResultJson = x.SearchResultJson
                });
        }
    }
}
