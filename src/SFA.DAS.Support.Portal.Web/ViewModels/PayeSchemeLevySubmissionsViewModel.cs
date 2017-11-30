﻿using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.Web.ViewModels
{
    public class PayeSchemeLevySubmissionsViewModel
    {
        public Account Account { get; set; }

        public decimal Balance { get; set; }

        public string SearchUrl { get; set; }
    }
}