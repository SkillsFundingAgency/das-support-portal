using System;
using ESFA.DAS.Support.Shared;

namespace ESFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IGetSiteManifest
    {
        SiteManifest GetSiteManifest(Uri siteUri);
    }
}