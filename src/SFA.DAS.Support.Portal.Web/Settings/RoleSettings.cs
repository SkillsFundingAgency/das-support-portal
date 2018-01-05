using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using SFA.DAS.Support.Portal.Core.Services;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    [ExcludeFromCodeCoverage]
    public class RoleSettings : IRoleSettings
    {
        [JsonRequired]
        public string ConsoleUserRole { get; set; }
        [JsonRequired]
        public string T2Role { get; set; }
        [JsonRequired]
        public bool ForceT2UserLocally { get; set; }
        [JsonRequired]
        public string GroupClaim { get; set; }
        [JsonRequired]
        public string Tier2Claim { get; set; }
    }
}