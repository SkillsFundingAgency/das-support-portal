using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IGetSearchItemsFromASite
    {
        Task<IEnumerable<T>> GetSearchItems<T>(Uri collectionUri);
    }
}