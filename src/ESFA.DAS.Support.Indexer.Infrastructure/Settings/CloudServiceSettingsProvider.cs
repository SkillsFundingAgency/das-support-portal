using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DAS.Support.Indexer.Core.Services;
using Microsoft.Azure;

namespace ESFA.DAS.Support.Indexer.Infrastructure.Settings
{
    public class CloudServiceSettingsProvider : IProvideSettings
    {
        private readonly IProvideSettings _baseSettings;

        public CloudServiceSettingsProvider(IProvideSettings baseSettings)
        {
            _baseSettings = baseSettings;
        }

        public string GetSetting(string settingKey)
        {
            var setting = GetNullableSetting(GetKey(settingKey));

            if (string.IsNullOrEmpty(setting))
            {
                throw new ConfigurationErrorsException($"Setting with key {settingKey} is missing");
            }

            return setting;
        }

        public string GetNullableSetting(string settingKey)
        {
            var setting = CloudConfigurationManager.GetSetting(GetKey(settingKey));

            if (string.IsNullOrWhiteSpace(setting))
            {
                setting = TryBaseSettingsProvider(settingKey);
            }

            return setting;
        }

        private string TryBaseSettingsProvider(string settingKey)
        {
            return _baseSettings.GetSetting(settingKey);
        }
        public IEnumerable<string> GetArray(string settingKey)
        {
            for (int i = 0; i < 100; i++)
            {
                var key = GetKey($"{settingKey}:{i}");
                var setting = CloudConfigurationManager.GetSetting(key);
                if (setting == null)
                {
                    break;
                }

                yield return setting;
            }
        }

        private string GetKey(string settingKey)
        {
            return settingKey.Replace(":", "_");
        }
    }
}
