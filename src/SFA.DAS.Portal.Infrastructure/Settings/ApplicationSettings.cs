using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Azure;
using SFA.DAS.Portal.Core.Configuration;

namespace SFA.DAS.Portal.Infrastructure.Settings
{
    public sealed class ApplicationSettings : IConfigurationSettings
    {
        public IEnumerable<Uri> ElasticServerUrls => GetElasticSearchIps();
        
        public string EnvironmentName => ConfigurationManager.AppSettings["EnvironmentName"];

        public string ApplicationName => ConfigurationManager.AppSettings["ApplicationName"];
        
        private IEnumerable<Uri> GetElasticSearchIps()
        {
            var urlStrings = CloudConfigurationManager.GetSetting("ElasticServerUrls").Split(',');
            return urlStrings.Select(url => new Uri(url));
        }
    }
}
