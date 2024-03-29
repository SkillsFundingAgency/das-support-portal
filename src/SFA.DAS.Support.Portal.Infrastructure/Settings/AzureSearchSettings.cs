﻿using SFA.DAS.Support.Portal.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public class AzureSearchSettings : IAzureSearchSettings
    {
        private readonly IProvideSettings _settings;

        public AzureSearchSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public string QueryApiKey => _settings.GetSetting("Support:Azure:Search:QueryKey");

        public string IndexName => _settings.GetSetting("Support:Azure:Search:IndexName");
        public string ServiceName => _settings.GetSetting("Support:Azure:Search:ServiceName");
    }
}
