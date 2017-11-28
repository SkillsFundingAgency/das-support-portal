using System.Collections.Generic;
using SFA.DAS.Portal.Core.Services;

namespace SFA.DAS.Portal.ApplicationServices.Settings
{
    public class SiteSettings : ISiteSettings
    {
        private readonly IProvideSettings _settings;

        public SiteSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<string> Sites => _settings.GetArray("Support:SubSite");
    }
}