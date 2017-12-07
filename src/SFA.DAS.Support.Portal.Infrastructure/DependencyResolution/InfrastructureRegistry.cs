using System.Net.Http;
using System.Threading.Tasks;
using HMRC.ESFA.Levy.Api.Client;
using Microsoft.Azure.Search;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerUsers.Api.Client;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Core.Configuration;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Portal.Infrastructure.Settings;
using SFA.DAS.TokenService.Api.Client;
using StructureMap.Configuration.DSL;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;

namespace SFA.DAS.Support.Portal.Infrastructure.DependencyResolution
{
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<ILoggingPropertyFactory>().Use<LoggingPropertyFactory>();

            For<IProvideSettings>().Use(c => new AppConfigSettingsProvider(new MachineSettings("DAS_")));
            For<ILog>().Use(x => new NLogLogger(
                   x.ParentType,
                   x.GetInstance<IRequestContext>(),
                   x.GetInstance<ILoggingPropertyFactory>().GetProperties())).AlwaysUnique();

            For<IConfigurationSettings>().Use<ApplicationSettings>();

            For<ITokenServiceApiClientConfiguration>().Use<LevySubmissionsApiConfiguration>();

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

            For<IEmployerUsersApiConfiguration>().Use<Settings.EmployerUsersApiConfiguration>();
            For<IAccountApiConfiguration>().Use<AccountsApiConfiguration>();
            
            For<IEmployerUserRepository>().Use<EmployerUserRepository>();
            For<IAccountRepository>().Use<AccountRepository>();
            For<ILevySubmissionsRepository>().Use<LevySubmissionsRepository>();
            For<IChallengeRepository>().Use<ChallengeRepository>();
            For<IHmrcClientConfiguration>().Use<HmrcClientConfiguration>();

            For<ISearchSettings>().Use<ElasticSearchSettings>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();
            For<ISearchProvider>().Use(x => new ElasticSearchProvider(x.GetInstance<IElasticsearchCustomClient>(), x.GetInstance<ISearchSettings>().IndexName));


            For<IEntityRepository>().Use<EntityRepository>();
            For<ISiteConnector>().Use<SiteConnector>();
            For<IFormMapper>().Use<FormMapper>();
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
