using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SiteConnection;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class DefaultRegistry : Registry
    {
        private static SupportServiceManifests _supportServiceManifests;
        private const string SupportServiceManifestsName = "SFA.DAS.Support.ServiceManifests";
        private const string ServiceName = "SFA.DAS.Support.Portal.Indexer.Worker";
        private const string Version = "1.0";

        public DefaultRegistry()
        {
            For<IRequestContext>().Use<FakeRequestContext>();

            For<ILoggingPropertyFactory>().Use<LoggingPropertyFactory>();
            For<ILog>().Use(x => new NLogLogger(
                x.ParentType,
                x.GetInstance<IRequestContext>(),
                x.GetInstance<ILoggingPropertyFactory>().GetProperties())).AlwaysUnique();


            var configuration = GetConfiguration();
            For<IWebConfiguration>().Use(configuration);
            For<ISearchSettings>().Use(configuration.ElasticSearch);
            For<ISiteSettings>().Use(configuration.Site);
            For<ISiteConnectorSettings>().Use(configuration.SiteConnector);
            _supportServiceManifests = GetManifests();
            For<SupportServiceManifests>().Use(_supportServiceManifests).Singleton();

        }

        private SupportServiceManifests GetManifests()
        {
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName");

            var storageConnectionString = CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString");

            if (environment == null) throw new ArgumentNullException(nameof(environment));

            if (storageConnectionString == null) throw new ArgumentNullException(nameof(storageConnectionString));


            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString); ;

            var configurationOptions = new ConfigurationOptions(SupportServiceManifestsName, environment, Version);

            var configurationService = new ConfigurationService(configurationRepository, configurationOptions);

            var configuration = configurationService.Get<SupportServiceManifests>();

            if (configuration == null) throw new ArgumentOutOfRangeException($"The requried {nameof(SupportServiceManifests)} configuration settings were not retrieved from {SupportServiceManifestsName}, please check the environmentName case, and the configuration connection string is correct.");

            return configuration;
        }

        private WebConfiguration GetConfiguration()
        {
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName") ?? "LOCAL";

            var storageConnectionString =
                CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString") ??
                "UseDevelopmentStorage=true;";

            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString);

            var configurationOptions = new ConfigurationOptions(ServiceName, environment, Version);

            var configurationService = new ConfigurationService(configurationRepository, configurationOptions);

            return configurationService.Get<WebConfiguration>();
        }
    }
}