using System.Collections.Generic;
using SFA.DAS.Support.Portal.Core.Services;

namespace SFA.DAS.Support.Portal.ApplicationServices.Settings
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