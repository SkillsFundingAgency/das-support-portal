using SFA.DAS.Support.Portal.Web.Api.Helpers;
using SFA.DAS.Support.Portal.Web.App_Start;
using SFA.DAS.Support.Portal.Web.Settings;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Web.Interfaces;
using SFA.DAS.Support.Portal.Web.Services;
using StructureMap.Configuration.DSL;
using SFA.DAS.Support.Portal.Web.Extensions;

namespace SFA.DAS.Support.Portal.Web.DependencyResolution
{
    /// <summary>
    /// DfESignInApiClientRegistry class to register the dependencies with IOC container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DfESignInApiClientRegistry : Registry
    {
        private const string ServiceName = "SFA.DAS.Provider.DfeSignIn";
        private const string Version = "1.0";

        public DfESignInApiClientRegistry()
        {
            var configuration = ServiceConfigurationExtension.GetConfiguration<DfESignInServiceConfiguration>(ServiceName, Version);

            For<DfESignInServiceConfiguration>().Use(configuration);
            For<IDfESignInServiceConfiguration>().Use(c => c.GetInstance<DfESignInServiceConfiguration>());
            For<IDfESignInService>().Use<DfESignInService>().Ctor<HttpClient>().Is(c => CreateClient());
            For<ITokenDataSerializer>().Use<TokenDataSerializer>();
            For<ITokenBuilder>().Use<TokenBuilder>();
        }

        /// <summary>
        /// Method to create the http client and inject it from constructor.
        /// </summary>
        /// <returns>HttpClient.</returns>
        private static HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken());
            return httpClient;
        }

        /// <summary>
        /// Method to generate the access token for DfESignIn Service.
        /// </summary>
        /// <returns>string.</returns>
        private static string AccessToken()
        {
            var tokenBuilder = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<ITokenBuilder>();
            if (tokenBuilder != null) return tokenBuilder.CreateToken();
            throw new NullReferenceException($"{nameof(tokenBuilder)} could not be null");
        }
    }
}