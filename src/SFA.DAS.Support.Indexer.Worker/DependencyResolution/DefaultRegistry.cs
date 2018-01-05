using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class DefaultRegistry : Registry
    {
        private const string ServiceName = "SFA.DAS.Support.Portal.Indexer.Worker";
        private const string Version = "1.0";
          public DefaultRegistry()
        {
            
            
            WebConfiguration configuration = GetConfiguration();

            For<IWebConfiguration>().Use(configuration);
            For<ISearchSettings>().Use(configuration.ElasticSearch);
            For<ISiteSettings>().Use(configuration.Site);
        }

        private WebConfiguration GetConfiguration()
        {
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName") ?? 
                                "LOCAL";
            var storageConnectionString = CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString") ??
                                "UseDevelopmentStorage=true;";

            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString); 

            var configurationOptions = new ConfigurationOptions(ServiceName, environment, Version);

            var configurationService = new ConfigurationService(configurationRepository, configurationOptions);

            return configurationService.Get<WebConfiguration>();
        }
    }
}