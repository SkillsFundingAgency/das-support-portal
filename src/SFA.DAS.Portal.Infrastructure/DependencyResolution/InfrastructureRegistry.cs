using System.Net.Http;
using System.Threading.Tasks;
using HMRC.ESFA.Levy.Api.Client;
using Microsoft.Azure.Search;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerUsers.Api.Client;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Portal.ApplicationServices;
using SFA.DAS.Portal.ApplicationServices.Services;
using SFA.DAS.Portal.Core.Configuration;
using SFA.DAS.Portal.Core.Services;
using SFA.DAS.Portal.Infrastructure.Services;
using SFA.DAS.Portal.Infrastructure.Settings;
using SFA.DAS.TokenService.Api.Client;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Portal.Infrastructure.DependencyResolution
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
            For<IAzureSearchSettings>().Use<AzureSearchSettings>();
            For<IAzureSearchProvider>().Use<AzureSearchProvider>();
            For<ISearchIndexClient>().Use("", c =>
            {
                var settings = c.GetInstance<IAzureSearchSettings>();
                return new SearchIndexClient(settings.ServiceName, settings.IndexName, new SearchCredentials(settings.QueryApiKey));
            });
            For<IEntityRepository>().Use<EntityRepository>();
            For<IDownload>().Use<WebDownloader>();
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
