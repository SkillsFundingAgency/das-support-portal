using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.Navigation;

namespace SFA.DAS.Support.Portal.ApplicationServices.Models
{
    [ExcludeFromCodeCoverage]
    public class NavViewModel
    {
        public SupportServiceResourceKey Current { get; set; }

        public NavItem[] Items { get; set; }
    }
}