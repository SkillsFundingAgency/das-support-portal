using Newtonsoft.Json;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.SiteConnection;
using System.Collections.Generic;

namespace SFA.DAS.Support.Indexer.Worker
{
    public class WebConfiguration : IWebConfiguration
    {
        [JsonRequired] public SiteSettings Site { get; set; }
        [JsonRequired] public ElasticSearchSettings ElasticSearch { get; set; }
        [JsonRequired] public List<SubSiteConnectorConfig> SubSiteConnectorSettings { get; set; }
    }
}