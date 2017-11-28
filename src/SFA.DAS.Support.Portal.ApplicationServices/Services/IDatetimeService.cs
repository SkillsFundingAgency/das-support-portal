using System;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public interface IDatetimeService
    {
        int GetYear(DateTime endDate);
        DateTime GetBeginningFinancialYear(DateTime endDate);
    }
}