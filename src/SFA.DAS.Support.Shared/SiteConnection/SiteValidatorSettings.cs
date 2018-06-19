using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    [ExcludeFromCodeCoverage]
    public class SiteValidatorSettings : ISiteValidatorSettings
    {
        [JsonRequired] public string Tenant { get; set; }

        [JsonRequired] public string Audience { get; set; }

        [JsonRequired] public string Scope { get; set; }
    }
}