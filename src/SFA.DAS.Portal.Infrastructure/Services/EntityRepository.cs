using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Portal.Core.Services;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Portal.Infrastructure.Services
{
    public class EntityRepository : IEntityRepository
    {
        private readonly IAzureSearchProvider _provider;

        public EntityRepository(IAzureSearchProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<string> Search(string query)
        {
            return _provider.Search<SearchItem>(query).Select(x => x.Html);
        }
    }
}
