using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    [ExcludeFromCodeCoverage]
    public class SiteSettings : ISiteSettings
    {

        [JsonRequired]
        public string BaseUrls { get; set; }

        [JsonRequired]
        public string EnvironmentName { get; set; }
    }
}