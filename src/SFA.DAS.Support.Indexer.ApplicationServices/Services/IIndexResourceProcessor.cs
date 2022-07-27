using System;
using System.Threading.Tasks;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IIndexResourceProcessor
    {
        Task ProcessResource(IndexResourceProcessorModel resourceProcessorModel);
    }

    public class IndexResourceProcessorModel
    {
        public Uri BasUri { get; set; }
        public SiteResource SiteResource { get; set; }
        public string ResourceIdentifier { get; set; }
    }
}