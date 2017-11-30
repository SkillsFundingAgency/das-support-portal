﻿using System.Collections.Generic;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    public class EmployerUserSearchResponse
    {
        public int Page { get; set; }

        public int LastPage { get; set; }

        public string SearchTerm { get; set; }
        
        public SearchResponseCodes StatusCode { get; set; }

        public IEnumerable<UserSummary> Results { get; set; }
    }
}