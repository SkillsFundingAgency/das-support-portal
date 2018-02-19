using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SFA.DAS.Support.Shared.SearchIndexModel
{
    [ExcludeFromCodeCoverage]
    public class AccountSearchModel : BaseSearchModel
    {
        public string Account { get; set; }
        public string AccountID { get; set; }
        public List<string> PayeSchemeIds { get; set; }


        public string AccountSearchKeyWord { get => Account.ToLower(); }
        public string AccountIDSearchKeyWord { get => AccountID.ToLower(); }

        public IEnumerable<string> PayeSchemeIdSearchKeyWords
        {
            get
            {
                return PayeSchemeIds?.Select(o => o.ToLower()).ToList();
            }
        }
    }
}