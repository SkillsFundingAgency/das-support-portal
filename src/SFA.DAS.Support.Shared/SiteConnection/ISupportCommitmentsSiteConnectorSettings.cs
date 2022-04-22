using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public interface ISupportCommitmentsSiteConnectorSettings
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string IdentifierUri { get; set; }
        string Tenant { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class SupportCommitmentsSiteConnectorSetting : ISupportCommitmentsSiteConnectorSettings
    {
        [JsonRequired] public string ClientId { get; set; }

        [JsonRequired] public string ClientSecret { get; set; }

        [JsonRequired] public string IdentifierUri { get; set; }

        [JsonRequired] public string Tenant { get; set; }
    }
}
