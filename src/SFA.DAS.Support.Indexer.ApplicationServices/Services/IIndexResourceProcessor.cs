using SFA.DAS.Support.Shared.SearchIndexModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IIndexResourceProcessor
    {
        Task ProcessResource(Uri uri, SearchCategory searchCategory);
    }
}
