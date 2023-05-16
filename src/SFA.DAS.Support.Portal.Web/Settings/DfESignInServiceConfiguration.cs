using Newtonsoft.Json;
using SFA.DAS.Support.Portal.Web.Interfaces;
using SFA.DAS.Support.Portal.Web.Models.SFA.DAS.ProviderApprenticeshipsService.Domain.Models;
using SFA.DAS.Support.Portal.Web.Models;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    /// <inheritdoc/>
    public class DfESignInServiceConfiguration : IDfESignInServiceConfiguration
    {
        public DfESignInConfig DfEOidcConfiguration { get; set; }

        [JsonProperty("DfEOidcConfiguration_SupportConsole")]
        public DfESignInClientConfig DfEOidcClientConfiguration { get; set; }
    }
}