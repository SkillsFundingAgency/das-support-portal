using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SFA.DAS.Support.Indexer.Core.Services;

namespace SFA.DAS.Support.Indexer.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public sealed class MachineSettings : IProvideSettings
    {
        private readonly string _prefix;

        public MachineSettings() : this(string.Empty)
        {
        }

        public MachineSettings(string prefix)
        {
            _prefix = prefix;
        }

        public string GetSetting(string settingKey)
        {
            return Environment.GetEnvironmentVariable(GetKey(settingKey), EnvironmentVariableTarget.User);
        }

        public string GetNullableSetting(string settingKey)
        {
            return GetSetting(settingKey);
        }

        public IEnumerable<string> GetArray(string settingKey)
        {
            var collection = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User).Keys;
            var enumerable = collection.Cast<string>();
            var arrayKeys = enumerable.Where(x => x.StartsWith($"{GetKey(settingKey)}:")).OrderBy(x => x);
            foreach (var key in arrayKeys)
                yield return Environment.GetEnvironmentVariable(GetKey(key), EnvironmentVariableTarget.User);
        }

        private string GetKey(string settingKey)
        {
            return $"{_prefix}{settingKey}";
        }
    }
}