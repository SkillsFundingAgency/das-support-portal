﻿using System;

namespace SFA.DAS.Support.Portal.Web.Models.Error
{
    public class Error403ViewModel
    {
        private readonly string _integrationUrlPart = string.Empty;

        public Error403ViewModel(string environment)
        {
            if (!string.IsNullOrEmpty(environment) && !environment.Equals("prd", StringComparison.CurrentCultureIgnoreCase))
            {
                _integrationUrlPart = "test-";
            }
        }

        /// <summary>
        /// Gets DfESignIn Select service link.
        /// </summary>
        public string HelpPageLink => $"https://{_integrationUrlPart}services.signin.education.gov.uk/approvals/select-organisation?action=request-service";

        /// <summary>
        /// Gets or Sets UseDfESignIn config property.
        /// </summary>
        public bool UseDfESignIn { get; set; }
    }
}