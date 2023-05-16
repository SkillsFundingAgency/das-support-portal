using Newtonsoft.Json;
using SFA.DAS.Support.Portal.Web.Models;
using SFA.DAS.Support.Portal.Web.Models.SFA.DAS.ProviderApprenticeshipsService.Domain.Models;

namespace SFA.DAS.Support.Portal.Web.Interfaces
{
    /// <summary>
    /// Contract to read the DfESignIn configuration from Azure Table Storage.
    /// </summary>
    public interface IDfESignInServiceConfiguration
    {
        DfESignInConfig DfEOidcConfiguration { get; set; }

        [JsonProperty("DfEOidcConfiguration_SupportConsole")]
        DfESignInClientConfig DfEOidcClientConfiguration { get; set; }
    }
}
