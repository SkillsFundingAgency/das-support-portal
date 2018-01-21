namespace SFA.DAS.Support.Shared
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