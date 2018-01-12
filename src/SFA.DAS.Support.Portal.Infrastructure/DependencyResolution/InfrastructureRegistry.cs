using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using HMRC.ESFA.Levy.Api.Client;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerUsers.Api.Client;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Portal.Infrastructure.Settings;
using SFA.DAS.TokenService.Api.Client;
using StructureMap.Configuration.DSL;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.Infrastructure.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<IEmployerUsersApiClient>().Use("", (ctx) =>
            {
                var empUserApiSettings = ctx.GetInstance<IEmployerUsersApiConfiguration>();
                return new EmployerUsersApiClient(empUserApiSettings);
            });
            For<IAccountApiClient>().Use("", (ctx) =>
            {
                var empUserApiSettings = ctx.GetInstance<IAccountApiConfiguration>();
                return new AccountApiClient(empUserApiSettings);
            });

            For<IApprenticeshipLevyApiClient>().Use("", (ctx) =>
            {
                var levySubmissionsApiConfiguration = ctx.GetInstance<ITokenServiceApiClientConfiguration>();
                var hmrcConfig = ctx.GetInstance<IHmrcClientConfiguration>();
                var httpClient = GetLevyHttpClient(levySubmissionsApiConfiguration, hmrcConfig);

                return new ApprenticeshipLevyApiClient(httpClient);
            });

            For<IEmployerUserRepository>().Use<EmployerUserRepository>();
            For<IAccountRepository>().Use<AccountRepository>();
            For<ILevySubmissionsRepository>().Use<LevySubmissionsRepository>();
            For<IChallengeRepository>().Use<ChallengeRepository>();

            For<IIndexNameCreator>().Use<IndexNameCreator>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();

            For<ISearchProvider>().Use<ElasticSearchProvider>();

            For<ISiteConnector>().Use<SiteConnector>();
            For<IFormMapper>().Use<FormMapper>();

            For<IWindowsLogonIdentityProvider>().Use<WindowsLogonIdentityProvider>();

        }

        private HttpClient GetLevyHttpClient(ITokenServiceApiClientConfiguration levySubmissionsApiConfiguration, IHmrcClientConfiguration hmrcClientConfiguration)
        {

            var tokenService = new TokenServiceApiClient(levySubmissionsApiConfiguration);

            var tokenResultTask = Task.Run(() => tokenService.GetPrivilegedAccessTokenAsync());
            tokenResultTask.Wait();

            return ApprenticeshipLevyApiClient.CreateHttpClient(tokenResultTask.Result.AccessCode, hmrcClientConfiguration.HttpClientBaseUrl);
        }
    }
}
