using Microsoft.Azure;
using SFA.DAS.Portal.Core.Services;

namespace SFA.DAS.Portal.Web.Settings
{
    public class RoleSettings : IRoleSettings
    {
        private readonly IProvideSettings _settings;

        public RoleSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public string ConsoleUserRole => "ConsoleUser";

        public string T2Role => "Tier2User";

        public bool ForceT2UserLocally => bool.Parse(_settings.GetSetting("ForceT2UserLocally"));

        public string GroupClaim => CloudConfigurationManager.GetSetting("ida_GroupClaim");

        public string Tier2Claim => CloudConfigurationManager.GetSetting("ida_Tier2Claim");
    }
}