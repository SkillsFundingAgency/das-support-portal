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
using SFA.DAS.EmployerUsers.Api.Client;
using SFA.DAS.Support.Portal.Infrastructure.Settings;
using SFA.DAS.Support.Portal.Web.Settings;

namespace SFA.DAS.Support.Portal.Web.DependencyResolution
{
    using Microsoft.Azure;
    using SFA.DAS.Configuration;
    using SFA.DAS.Configuration.AzureTableStorage;
    using SFA.DAS.EAS.Account.Api.Client;
    using SFA.DAS.Support.Common.Infrastucture.Settings;
    using SFA.DAS.Support.Portal.ApplicationServices.Settings;
    using SFA.DAS.Support.Portal.Core.Services;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class DefaultRegistry : Registry
    {
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
            
            WebConfiguration configuration = GetConfiguration();

            For<IWebConfiguration>().Use(configuration);
            For<IChallengeSettings>().Use(configuration.Challenge);
            For<ISearchSettings>().Use(configuration.ElasticSearch);
            For<IHmrcClientConfiguration>().Use(configuration.HmrcClient);
            For<IAccountApiConfiguration>().Use(configuration.AccountsApi);
            For<IEmployerUsersApiConfiguration>().Use(configuration.EmployerUsersApi);
            For<ILevySubmissionsApiConfiguration>().Use(configuration.LevySubmissionsApi);
            For<ISiteConnectorSettings>().Use(configuration.SiteConnector);
            For<ISiteSettings>().Use(configuration.Site);
            For<IRoleSettings>().Use(configuration.Roles);
            For<IAuthSettings>().Use(configuration.Authentication);
            For<ICryptoSettings>().Use(configuration.Crypto);
        }

        private WebConfiguration GetConfiguration()
        {
            var environment = CloudConfigurationManager.GetSetting("EnvironmentName") ?? 
                                "AT";

            var storageConnectionString = CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString") ??
                                "UseDevelopmentStorage=true;";


            
            
            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString); ;

            var configurationOptions = new ConfigurationOptions(ServiceName, environment, Version);


            throw new ArgumentNullException($"configuration Variables are not set correctly: ServiceName: {ServiceName} environment: {environment} Version: {Version} storageConnectionString: {storageConnectionString}");

            
            var configurationService = new ConfigurationService(configurationRepository, configurationOptions);

            return configurationService.Get<WebConfiguration>();
        }

       
    }
}