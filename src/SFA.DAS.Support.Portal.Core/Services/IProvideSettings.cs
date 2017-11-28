using System.Collections.Generic;

namespace SFA.DAS.Support.Portal.Core.Services
{
    public interface IProvideSettings
    {
        string GetSetting(string settingKey);
        string GetNullableSetting(string settingKey);
        IEnumerable<string> GetArray(string settingKey);
    }
}
