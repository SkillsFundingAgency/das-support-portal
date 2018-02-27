using System;
using System.Threading.Tasks;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IIndexResourceProcessor
    {
        Task ProcessResource(Uri basUri, SiteResource siteResource);
    }
}