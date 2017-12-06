using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Core.Services;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    [ExcludeFromCodeCoverage]
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