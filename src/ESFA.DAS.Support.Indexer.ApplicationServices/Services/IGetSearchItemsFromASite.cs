using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DAS.Support.Shared;

namespace ESFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IGetSearchItemsFromASite
    {
        Task<IEnumerable<SearchItem>> GetSearchItems(Uri collectionUri);
    }
}