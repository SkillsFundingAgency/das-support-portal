using System.Collections.Generic;
using SFA.DAS.Support.Portal.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.ApplicationServices.Settings
{
    [ExcludeFromCodeCoverage]
    public class SiteSettings : ISiteSettings
    {
        private readonly IProvideSettings _settings;

        public SiteSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<string> Sites
        {
            get
            {
                var sites = _settings.GetArray("Support:SubSite");
                return  sites;
            }
        }
    }
}