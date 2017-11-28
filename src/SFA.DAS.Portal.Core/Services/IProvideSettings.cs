using System.Collections.Generic;

namespace SFA.DAS.Portal.Core.Services
{
    public interface IProvideSettings
    {
        string GetSetting(string settingKey);
        string GetNullableSetting(string settingKey);
        IEnumerable<string> GetArray(string settingKey);
    }
}
