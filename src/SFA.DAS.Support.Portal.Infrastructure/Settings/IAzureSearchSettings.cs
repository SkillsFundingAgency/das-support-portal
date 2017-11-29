namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    public interface IAzureSearchSettings
    {
        string QueryApiKey { get; }
        string IndexName { get; }
        string ServiceName { get; }
    }
}