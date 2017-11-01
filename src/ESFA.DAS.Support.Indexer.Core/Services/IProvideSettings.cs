using System.Collections.Generic;

namespace ESFA.DAS.Support.Indexer.Core.Services
{
    public interface IProvideSettings
    {
        IEnumerable<string> GetArray(string settingKey);
        string GetSetting(string settingKey);
        string GetNullableSetting(string settingKey);
    }
}