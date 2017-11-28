using System.Collections;
using NUnit.Framework;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.UnitTests.Handlers
{
    public class UserStatusTestDataClass
    {
        public static IEnumerable UserStatusTestCases
        {
            get { yield return new TestCaseData(false, true).SetName("CheckThatAnInActiveAndLockedUserHasAnUnverifiedStatus").Returns(UserStatus.Unverified); }
        }
    }
}