using System;
using System.Globalization;
using HMRC.ESFA.Levy.Api.Types;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Portal.Web.Helpers
{
    public class AccountHelper
    {
        public static string GetTeamMemberStatus(InvitationStatus status)
        {
            switch (status)
            {
                case InvitationStatus.Accepted:
                    return "Active";
                case InvitationStatus.Pending:
                    return "Invitation awaiting response";
                case InvitationStatus.Expired:
                    return "Invitation expired";
            }

            return string.Empty;
        }

        public static string GetSubmissionDate(DateTime submissionTime)
        {
            return submissionTime.ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        public static string GetLevyDeclarationDescription(Declaration levyDeclaration)
        {
            if (levyDeclaration.DateCeased.HasValue && levyDeclaration.DateCeased.Value != DateTime.MinValue)
            {
                return $"Ceased {ConvertDateTimeToDdmmyyyyFormat(levyDeclaration.DateCeased)}";
            }

            if (levyDeclaration.NoPaymentForPeriod)
            {
                return "No payment";
            }

            if (levyDeclaration.InactiveFrom.HasValue && levyDeclaration.InactiveFrom.Value != DateTime.MinValue)
            {
                return $"Inactive {ConvertDateTimeToDdmmyyyyFormat(levyDeclaration.InactiveFrom)} "
                    + $"to {ConvertDateTimeToDdmmyyyyFormat(levyDeclaration.InactiveTo)}";
            }

            return string.Empty;
        }

        public static string ConvertDateTimeToDdmmyyyyFormat(DateTime? dateTime)
        {
            return dateTime?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static string GetYearToDateAmount(Declaration levyDeclaration)
        {
            if (levyDeclaration.NoPaymentForPeriod || 
                (levyDeclaration.DateCeased.HasValue && levyDeclaration.DateCeased != DateTime.MinValue) ||
                (levyDeclaration.InactiveFrom.HasValue && levyDeclaration.InactiveFrom.Value != DateTime.MinValue))
            {
                return string.Empty;
            }
            return $"£{levyDeclaration.LevyDueYearToDate.ToString("#,##0.00")}";
        }

        public static string GetPayrollDate(PayrollPeriod payrollPeriod)
        {
            if (payrollPeriod == null)
            {
                return string.Empty;
            }

            var month = payrollPeriod.Month + 3;

            if (month > 12)
            {
                month = month - 12;
            }

            var monthName = new DateTime(2010, month, 1)
                .ToString("MMM", CultureInfo.InvariantCulture);

            return $"{payrollPeriod.Year} {payrollPeriod.Month} ({monthName})";
        }
    }
}