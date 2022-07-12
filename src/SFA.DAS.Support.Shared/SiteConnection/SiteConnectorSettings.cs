﻿using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorSettings : ISiteConnectorSettings
    {
        [JsonRequired] public string ClientId { get; set; }

        [JsonRequired] public string ClientSecret { get; set; }

        [JsonRequired] public string IdentifierUri { get; set; }

        [JsonRequired] public string Tenant { get; set; }
    }


    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorMISettings : ISiteConnectorMISettings
    {
        public SiteConnectorMI SupportCommitmentsSiteConnector { get; set; }
        public SiteConnectorMI SupportEASSiteConnector { get; set; }
        public SiteConnectorMI SupportEmployerUsersSiteConnector { get; set; }
    }
}