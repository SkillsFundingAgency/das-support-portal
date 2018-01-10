using System.Linq;
using System.Collections.Generic;
using SFA.DAS.Support.Common.Infrastucture.Models;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Common.Infrastucture.Indexer
{
    public interface ISearchProvider
    {
        PagedSearchResponse<UserSearchModel> FindUsers(string searchText, SearchCategory searchType, int pageSize = 10, int pageNumber = 0);

        PagedSearchResponse<AccountSearchModel> FindAccounts(string searchText, SearchCategory searchType, int pageSize = 10, int pageNumber = 0);
    }

}
