using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.Web.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class FinanceViewModel
    {
        public Account Account { get; set; }

        public decimal Balance { get; set; }

        public string SearchUrl { get; set; }
    }
}