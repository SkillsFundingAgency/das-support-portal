using SFA.DAS.Portal.Core.Services;

namespace SFA.DAS.Portal.Infrastructure.Settings
{
    public class HmrcClientConfiguration : IHmrcClientConfiguration
    {
        private readonly IProvideSettings _settings;

        public HmrcClientConfiguration(IProvideSettings settings)
        {
            _settings = settings;
        }

        public string HttpClientBaseUrl => _settings.GetSetting("LevyHttpClientBaseUrl");
    }
}