using System.Collections.Generic;
using ESFA.DAS.Support.Indexer.Core.Services;

namespace ESFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    public class SiteSettings : ISiteSettings
    {
        private readonly IProvideSettings _settings;

        public SiteSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<string> Sites => _settings.GetArray("Support:SubSite");

        public string IndexName =>
            string.Format(_settings.GetSetting("IndexNameFormat"), _settings.GetSetting("EnvironmentName"));
    }
}