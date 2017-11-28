using SFA.DAS.Support.Indexer.Core.Services;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    public class SearchSettings : ISearchSettings
    {
        private readonly IProvideSettings _settings;

        public SearchSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public string AdminApiKey => _settings.GetSetting("Support:Azure:Search:AdminKey");
        public string ServiceName => _settings.GetSetting("Support:Azure:Search:ServiceName");
    }
}