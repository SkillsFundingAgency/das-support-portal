using System;
using System.Collections.Generic;
using ESFA.DAS.Support.Shared;

namespace ESFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IGetSearchItemsFromASite
    {
        IEnumerable<SearchItem> GetSearchItems(Uri collectionUri);
    }
}