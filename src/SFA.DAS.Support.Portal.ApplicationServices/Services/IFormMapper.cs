using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public interface IFormMapper
    {
        string UpdateForm(SupportServiceResourceKey resourceKey, SupportServiceResourceKey challengeKey, string id, string url, string html);
    }
}