namespace SFA.DAS.Portal.Infrastructure.Settings
{
    public interface IAzureSearchSettings
    {
        string QueryApiKey { get; }
        string IndexName { get; }
        string ServiceName { get; }
    }
}