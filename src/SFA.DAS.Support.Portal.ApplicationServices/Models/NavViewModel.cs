using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Shared.Navigation;

namespace SFA.DAS.Support.Portal.ApplicationServices.Models
{
    [ExcludeFromCodeCoverage]
    public class NavViewModel
    {
        public string Current { get; set; }

        public NavItem[] Items { get; set; }
    }
}