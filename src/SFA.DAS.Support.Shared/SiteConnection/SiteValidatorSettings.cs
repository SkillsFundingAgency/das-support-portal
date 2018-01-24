using Newtonsoft.Json;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class SiteValidatorSettings : ISiteValidatorSettings
    {
        [JsonRequired] public string Tenant { get; set; }

        [JsonRequired] public string Audience { get; set; }

        [JsonRequired] public string Scope { get; set; }
    }
}