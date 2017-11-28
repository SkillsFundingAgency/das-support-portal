using System;

namespace SFA.DAS.Portal.ApplicationServices.Services
{
    public interface IDatetimeService
    {
        int GetYear(DateTime endDate);
        DateTime GetBeginningFinancialYear(DateTime endDate);
    }
}