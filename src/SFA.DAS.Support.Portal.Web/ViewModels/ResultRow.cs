using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Web.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ResultRow
    {
        public ResultRow()
        {
            CellValues = new List<ResultCell>();
        }

        public List<ResultCell> CellValues { get; set; }
    }
}