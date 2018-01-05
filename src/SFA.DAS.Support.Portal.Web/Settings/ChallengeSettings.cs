using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    [ExcludeFromCodeCoverage]
    public class ChallengeSettings : IChallengeSettings
    {
        public int ChallengeTimeoutMinutes { get; set; } // => int.Parse(_settings.GetSetting("ChallengeTimeout"));
    }
}