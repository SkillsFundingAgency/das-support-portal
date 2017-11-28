using SFA.DAS.Portal.Core.Services;

namespace SFA.DAS.Portal.Web.Settings
{
    public class ChallengeSettings : IChallengeSettings
    {
        private readonly IProvideSettings _settings;

        public ChallengeSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public int ChallengeTimeoutMinutes => int.Parse(_settings.GetSetting("ChallengeTimeout"));
    }
}