using System.Collections.Generic;

namespace SFA.DAS.Support.Portal.Web.ViewModels
{
    public class ResultRow
    {
        public ResultRow()
        {
            CellValues = new List<ResultCell>();
        }

        public List<ResultCell> CellValues { get; set; }
    }
}