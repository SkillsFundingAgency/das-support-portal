using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    [ExcludeFromCodeCoverage]
    public class AuthSettings : IAuthSettings
    {
        [JsonRequired]
        public string AdfsMetadata { get; set; }
        [JsonRequired]
        public string Realm { get; set; }
    }
}