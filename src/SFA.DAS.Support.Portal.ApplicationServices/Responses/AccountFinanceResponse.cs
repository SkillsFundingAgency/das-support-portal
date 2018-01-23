using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    [ExcludeFromCodeCoverage]
    public class AccountFinanceResponse
    {
        public Account Account { get; set; }

        public decimal Balance { get; set; }

        public SearchResponseCodes StatusCode { get; set; }
    }
}