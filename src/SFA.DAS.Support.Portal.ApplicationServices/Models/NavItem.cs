using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.ApplicationServices.Models
{
    [ExcludeFromCodeCoverage]
    public class NavItem
    {
        public string Title { get; set; }
        public string Href { get; set; }
        public string Key { get; set; }
    }
}
