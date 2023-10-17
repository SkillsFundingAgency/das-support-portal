namespace SFA.DAS.Support.Portal.Web.Models
{
    namespace SFA.DAS.ProviderApprenticeshipsService.Domain.Models
    {
        /// <summary>
        /// model to hold configurations of DfESignIn
        /// </summary>
        public class DfESignInConfig
        {
            /// <summary>
            /// Gets or Sets BaseUrl
            /// </summary>
            public string BaseUrl { get; set; }

            /// <summary>
            /// Gets or Sets Api Service Url.
            /// </summary>
            public string ApiServiceUrl { get; set; }

            /// <summary>
            /// Gets or Sets Scopes.
            /// </summary>
            public string Scopes { get; set; }
        }
    }
}