namespace SFA.DAS.Support.Shared.SiteConnection
{
    public interface ISiteConnectorSettings
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string IdentifierUri { get; set; }
        string Tenant { get; set; }
    }
}