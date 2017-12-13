using SFA.DAS.Support.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFA.DAS.Support.Portal.Web.ViewModels
{
    public class SearchTableResultViewModel
    {
        public SearchTableResultViewModel()
        {
            Columns = new List<string>();
            Rows = new List<ResultRow>();
        }

        public List<string> Columns { get; set; }

        public List<ResultRow> Rows { get; set; }

    }

}