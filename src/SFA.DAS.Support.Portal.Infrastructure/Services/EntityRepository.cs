using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ISearchProvider _provider;

        public EntityRepository(ISearchProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<string> Search(string query)
        {
            return _provider.Search<SearchItem>(query).Select(x => x.Html);
        }
    }
}
