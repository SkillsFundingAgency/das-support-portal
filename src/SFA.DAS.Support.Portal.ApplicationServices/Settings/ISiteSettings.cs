namespace SFA.DAS.Support.Portal.ApplicationServices.Settings
{
    public interface ISiteSettings
    {
        /// <summary>
        ///     A comma separated list of site base Url's e.g.
        ///     https://localhost:44312,https://localhost:44313
        /// </summary>
        string BaseUrls { get; set; }
    }
}