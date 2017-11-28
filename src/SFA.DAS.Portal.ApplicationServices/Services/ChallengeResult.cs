namespace SFA.DAS.Portal.ApplicationServices.Services
{
    public class ChallengeResult
    {
        public bool HasRedirect => !string.IsNullOrEmpty(RedirectUrl);
        public string Page { get; set; }
        public string RedirectUrl { get; set; }
    }
}