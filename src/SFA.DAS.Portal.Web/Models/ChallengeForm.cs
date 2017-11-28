using System.Web.Mvc;

namespace SFA.DAS.Portal.Web.Models
{
    public class ChallengeForm
    {
        public string Redirect { get; set; }
        public string InnerAction { get; set; }
        public string Key { get; set; }
        public FormCollection Form { get; set; }
        public string ResourceKey { get; set; }
    }
}