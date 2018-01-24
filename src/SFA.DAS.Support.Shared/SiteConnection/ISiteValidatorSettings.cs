namespace SFA.DAS.Support.Shared.SiteConnection
{
    public interface ISiteValidatorSettings
    {
        string Tenant { get; set; }
        string Audience { get; set; }
        string Scope { get; set; }
    }
}