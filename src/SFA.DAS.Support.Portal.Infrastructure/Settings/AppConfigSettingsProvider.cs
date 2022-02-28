using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Azure;
using SFA.DAS.Support.Portal.Core.Services;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public class AppConfigSettingsProvider : IProvideSettings
    {
        private readonly IProvideSettings _baseSettings;

        public AppConfigSettingsProvider(IProvideSettings baseSettings)
        {
            _baseSettings = baseSettings;
        }

        public string GetSetting(string settingKey)
        {
            var setting = GetNullableSetting(settingKey);

            if (string.IsNullOrEmpty(setting))
            {
                throw new ConfigurationErrorsException($"Setting with key {settingKey} is missing");
            }

            return setting;
        }

        public string GetNullableSetting(string settingKey)
        {
            var setting = ConfigurationManager.AppSettings(GetKey(settingKey))
                ?? ConfigurationManager.AppSettings[settingKey];

            if (string.IsNullOrWhiteSpace(setting))
            {
                setting = TryBaseSettingsProvider(settingKey);
            }

            return setting;
        }

        public IEnumerable<string> GetArray(string settingKey)
        {
            var list = GetCloudArray(settingKey).ToList();

            if (list.Any())
            {
                return list;
            }

            return _baseSettings.GetArray(settingKey);

        }

        private IEnumerable<string> GetCloudArray(string settingKey)
        {
            for (int i = 0; i < 100; i++)
            {
                var key = GetKey($"{settingKey}:{i}");
                var setting = ConfigurationManager.AppSettings(key);
                if (setting == null)
                {
                    break;
                }

                yield return setting;
            }
        }

        private string TryBaseSettingsProvider(string settingKey)
        {
            return _baseSettings.GetSetting(settingKey);
        }

        private string GetKey(string settingKey)
        {
            return settingKey.Replace(":", "_");
        }
    }
}
