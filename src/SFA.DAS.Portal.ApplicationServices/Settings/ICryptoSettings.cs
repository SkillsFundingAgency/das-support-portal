namespace SFA.DAS.Portal.ApplicationServices.Settings
{
    public interface ICryptoSettings
    {
        string Salt { get; }
        string Secret { get; }
    }
}