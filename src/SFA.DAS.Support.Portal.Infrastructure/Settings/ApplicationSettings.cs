﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Azure;
using SFA.DAS.Support.Portal.Core.Configuration;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public sealed class ApplicationSettings : IConfigurationSettings
    {
        public IEnumerable<Uri> ElasticServerUrls => GetElasticSearchIps();

        public string EnvironmentName => ConfigurationManager.AppSettings["EnvironmentName"];

        public string ApplicationName => ConfigurationManager.AppSettings["ApplicationName"];

        private IEnumerable<Uri> GetElasticSearchIps()
        {
            var urlStrings = ConfigurationManager.AppSettings("ElasticServerUrls").Split(',');
            return urlStrings.Select(url => new Uri(url));
        }
    }
}
