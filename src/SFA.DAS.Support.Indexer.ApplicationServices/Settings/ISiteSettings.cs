namespace SFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    public interface ISiteSettings
    {
        string BaseUrls { get; }
        string EnvironmentName { get; set; }
    }
}