using System.Collections.Generic;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Common.Infrastucture.Indexer
{
    public interface ISearchProvider
    {
        IEnumerable<SearchItem> Search<SeachItem>(string searchText, int top = 50, int skip = 0);
    }
}
