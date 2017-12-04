using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace SFA.DAS.Support.Portal.Web.Models
{
    //[ExcludeFromCodeCoverage]
    public class ChallengeForm
    {
        public string Redirect { get; set; }
        public string InnerAction { get; set; }
        public string Key { get; set; }
        public FormCollection Form { get; set; }
        public string ResourceKey { get; set; }
    }
}