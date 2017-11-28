using Microsoft.Azure;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    public class AuthSettings : IAuthSettings
    {
        public string Realm => CloudConfigurationManager.GetSetting("ida_Wtrealm");
        public string AdfsMetadata => CloudConfigurationManager.GetSetting("ida_ADFSMetadata")?.Trim();
    }
}