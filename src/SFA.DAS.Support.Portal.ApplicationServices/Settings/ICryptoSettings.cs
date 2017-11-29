namespace SFA.DAS.Support.Portal.ApplicationServices.Settings
{
    public interface ICryptoSettings
    {
        string Salt { get; }
        string Secret { get; }
    }
}