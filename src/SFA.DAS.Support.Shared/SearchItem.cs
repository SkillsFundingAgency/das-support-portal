using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Shared
{
    [ExcludeFromCodeCoverage]
    public class SearchItem
    {
        [Key]
        public string SearchId { get; set; }

        public string[] Keywords { get; set; }

        public string Html { get; set; }
    }
}