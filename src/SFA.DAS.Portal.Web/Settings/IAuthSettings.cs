namespace SFA.DAS.Portal.Web.Settings
{
    public interface IAuthSettings
    {
        string AdfsMetadata { get; }
        string Realm { get; }
    }
}