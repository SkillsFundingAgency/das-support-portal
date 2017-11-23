using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Support.Shared
{
    public class SearchItem
    {
        [Key]
        public string SearchId { get; set; }

        public string[] Keywords { get; set; }

        public string Html { get; set; }
    }
}