namespace SFA.DAS.Support.Portal.ApplicationServices.Settings
{
    public interface ICryptoSettings
    {
        string Salt { get; set; }
        string Secret { get; set; }
    }
}