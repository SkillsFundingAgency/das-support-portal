using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SFA.DAS.Support.Portal.Health.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        Green = 0, Red = 1
    }
}