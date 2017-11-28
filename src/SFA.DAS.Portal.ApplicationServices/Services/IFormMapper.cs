namespace SFA.DAS.Portal.ApplicationServices.Services
{
    public interface IFormMapper
    {
        string UpdateForm(string key, string id, string url, string html);
    }
}