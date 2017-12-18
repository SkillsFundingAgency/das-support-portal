namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    public interface ISiteConnectorSettings
    {
        string ClientId { get; }
        string AppKey { get; }
        string ResourceId { get; }
        string Tenant { get; }
    }
}