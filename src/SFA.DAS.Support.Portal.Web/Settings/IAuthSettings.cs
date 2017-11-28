namespace SFA.DAS.Support.Portal.Web.Settings
{
    public interface IAuthSettings
    {
        string AdfsMetadata { get; }
        string Realm { get; }
    }
}