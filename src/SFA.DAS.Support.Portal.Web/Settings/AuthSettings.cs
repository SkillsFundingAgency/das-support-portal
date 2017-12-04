using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    //[ExcludeFromCodeCoverage]
    public class AuthSettings : IAuthSettings
    {
        public string Realm => CloudConfigurationManager.GetSetting("ida_Wtrealm");
        public string AdfsMetadata => CloudConfigurationManager.GetSetting("ida_ADFSMetadata")?.Trim();
    }
}