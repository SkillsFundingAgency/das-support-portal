// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using SFA.DAS.Support.Portal.Web.Settings;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.Web.DependencyResolution
{
    using Microsoft.Azure;
    using SFA.DAS.Configuration;
    using SFA.DAS.Configuration.AzureTableStorage;
    using SFA.DAS.NLog.Logger;
    using SFA.DAS.Support.Common.Infrastucture.Settings;
    using SFA.DAS.Support.Portal.ApplicationServices.Settings;
    using SFA.DAS.Support.Portal.Core.Services;
    using SFA.DAS.Support.Portal.Infrastructure.DependencyResolution;
    using SFA.DAS.Support.Shared.Discovery;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class DefaultRegistry : Registry
    {
        private static ServiceConfiguration _supportServiceConfiguration;
        private const string SupportServiceManifestsName = "SFA.DAS.Support.ServiceManifests";
        private const string ServiceName = "SFA.DAS.Support.Portal";
        private const string Version = "1.0";


        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            For<ILoggingPropertyFactory>().Use<LoggingPropertyFactory>();
            For<ILog>().Use(x => new NLogLogger(
                x.ParentType,
                x.GetInstance<IRequestContext>(),
                x.GetInstance<ILoggingPropertyFactory>().GetProperties())).AlwaysUnique();



            WebConfiguration configuration = GetConfiguration();

            For<IWebConfiguration>().Use(configuration);
            For<IAuthSettings>().Use(configuration.Authentication);
            For<IChallengeSettings>().Use(configuration.Challenge);
            For<ICryptoSettings>().Use(configuration.Crypto);
            For<ISearchSettings>().Use(configuration.ElasticSearch);
            For<ISiteConnectorSettings>().Use(configuration.SiteConnector);
            For<ISiteSettings>().Use(configuration.Site);
            For<IRoleSettings>().Use(configuration.Roles);

            For<IADFSConfiguration>().Use<ADFSConfiguration>();

           
            For<ServiceConfiguration>().Use<ServiceConfiguration>().Singleton();

        }


        private WebConfiguration GetConfiguration()
        {
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName");

            var storageConnectionString = CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString");

            if (environment == null) throw new ArgumentNullException(nameof(environment));
            if (storageConnectionString == null) throw new ArgumentNullException(nameof(storageConnectionString));


            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString); ;

            var configurationOptions = new ConfigurationOptions(ServiceName, environment, Version);

            var configurationService = new ConfigurationService(configurationRepository, configurationOptions);

            var webConfiguration = configurationService.Get<WebConfiguration>();

            if (webConfiguration == null) throw new ArgumentOutOfRangeException($"The requried configuration settings were not retrieved, please check the environmentName case, and the configuration connection string is correct.");

            return webConfiguration;
        }
    }
}