using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorSettings : ISiteConnectorSettings
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string IdentifierUri { get; set; }

        public string Tenant { get; set; }
    }
}