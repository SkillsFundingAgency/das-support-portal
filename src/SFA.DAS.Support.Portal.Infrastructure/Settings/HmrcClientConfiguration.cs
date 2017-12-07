using SFA.DAS.Support.Portal.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
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