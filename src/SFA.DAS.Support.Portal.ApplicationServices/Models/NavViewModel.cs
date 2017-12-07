using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.ApplicationServices.Models
{
    //[ExcludeFromCodeCoverage]
    public class NavViewModel
    {
        public string Current { get; set; }

        public NavItem[] Items { get; set; }
    }
}