using System.Collections.Generic;
using SFA.DAS.Support.Indexer.Core.Services;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    public class SiteSettings : ISiteSettings
    {
        private readonly IProvideSettings _settings;

        public SiteSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<string> Sites => _settings.GetArray("Support:SubSite");

        public string EnvironmentName => _settings.GetSetting("Support:Service:Environment");
    }
}