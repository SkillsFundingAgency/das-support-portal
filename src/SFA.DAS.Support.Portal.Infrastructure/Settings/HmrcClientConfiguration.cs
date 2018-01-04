using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public class HmrcClientConfiguration : IHmrcClientConfiguration
    {
        
        [JsonRequired]
        public string HttpClientBaseUrl { get; set; }
    }
}