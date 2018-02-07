using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Shared.Navigation
{
    [ExcludeFromCodeCoverage]
    public class NavItem
    {
        public string Title { get; set; }
        public string Href { get; set; }
        public SupportServiceResourceKey Key { get; set; }
    }
}