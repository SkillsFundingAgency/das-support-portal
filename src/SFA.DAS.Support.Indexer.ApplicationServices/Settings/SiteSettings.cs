using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    [ExcludeFromCodeCoverage]
    public class SiteSettings : ISiteSettings
    {
        [JsonRequired] public string DelayTimeInSeconds { get; set; }
    }
}