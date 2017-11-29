using System;
using System.Threading.Tasks;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IGetSiteManifest
    {
        Task<SiteManifest> GetSiteManifest(Uri siteUri);
    }
}