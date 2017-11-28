using SFA.DAS.Support.Portal.Core.Services;

namespace SFA.DAS.Support.Portal.ApplicationServices.Settings
{
    public class CryptoSettings : ICryptoSettings
    {
        private readonly IProvideSettings _settings;

        public CryptoSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public string Salt => _settings.GetSetting("CryptoSalt");
        public string Secret => _settings.GetSetting("CryptoSecret");
    }
}