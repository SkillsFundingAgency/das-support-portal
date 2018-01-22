namespace SFA.DAS.Support.Shared.SiteConnection
{
    public interface ISiteConnectorSettings
    {
        string ApiBaseUrl { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string IdentifierUri { get; set; }
        string Tenant { get; set; }

        string Audience { get; set; }
        string Scope { get; set; }
    }
}