﻿using Newtonsoft.Json;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.SiteConnection;
using System.Collections.Generic;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    public class WebConfiguration : IWebConfiguration
    {
        [JsonRequired] public AuthSettings Authentication { get; set; }
        [JsonRequired] public CryptoSettings Crypto { get; set; }
        [JsonRequired] public ChallengeSettings Challenge { get; set; }
        [JsonRequired] public ElasticSearchSettings ElasticSearch { get; set; }
        [JsonRequired] public RoleSettings Roles { get; set; }
        [JsonRequired] public List<SubSiteConnectorConfig> SubSiteConnectorSettings { get; set; }

        // <inherit-doc />
        [JsonRequired] public bool UseDfESignIn { get; set; }
    }
}