using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Configuration;
using System;
using System.Configuration;

namespace SFA.DAS.Support.Portal.Web.Extensions
{
    public class ServiceConfigurationExtension
    {
        public static TResponse GetConfiguration<TResponse>(string serviceName, string version) where TResponse : class
        {
            var environment = ConfigurationManager.AppSettings["EnvironmentName"];

            var storageConnectionString = ConfigurationManager.AppSettings["ConfigurationStorageConnectionString"];

            if (environment == null) throw new ArgumentNullException(nameof(environment));

            if (storageConnectionString == null) throw new ArgumentNullException(nameof(storageConnectionString));

            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString);

            var configurationOptions = new ConfigurationOptions(serviceName, environment, version);

            var configurationService = new ConfigurationService(configurationRepository, configurationOptions);

            var configuration = configurationService.Get<TResponse>();

            if (configuration == null) 
                throw new ArgumentOutOfRangeException($"The requried configuration settings were not retrieved, please check the environmentName case, and the configuration connection string is correct.");

            return configuration;
        }
    }
}