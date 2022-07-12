using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Portal.ApplicationServices.Settings
{
    [ExcludeFromCodeCoverage]
    public class SiteSettings : ISiteSettings
    {
        public string BaseUrls { get; set; }
    }
}