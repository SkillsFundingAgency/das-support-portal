namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    public interface ISiteConnectorSettings
    {
        string ApiBaseUrl { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string IdentifierUri { get; set; }
        string Tenant { get; set; }
    }
}