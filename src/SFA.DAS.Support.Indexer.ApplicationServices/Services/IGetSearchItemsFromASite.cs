using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IGetSearchItemsFromASite
    {
        Task<IEnumerable<T>> GetSearchItems<T>(Uri collectionUri);
    }
}