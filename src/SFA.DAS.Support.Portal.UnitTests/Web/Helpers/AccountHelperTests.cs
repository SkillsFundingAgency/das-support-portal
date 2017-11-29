using System;
using FluentAssertions;
using HMRC.ESFA.Levy.Api.Types;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Support.Portal.Web.Helpers;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Helpers
{
    [TestFixture]
    public sealed class AccountHelperTests
    {
        [TestCase("", null, false, null, null)]
        [TestCase("No payment", null, true, null, null)]
        [TestCase("Ceased 01/07/2017", "01/07/2017", false, null, null)]
        [TestCase("", "01/01/0001", false, null, null)]
        [TestCase("Inactive 01/07/2017 to 21/07/2017", null, false, "01/07/2017", "21/07/2017")]
        [TestCase("", null, false, "01/01/0001", "01/01/0001")]
        public void ShouldReturnLevyDeclarationDescription(string expected, string dateCeased, bool noPaymentForPeriod, string inactiveFrom, string inactiveTo)
        {
            var levyDeclaration = new Declaration
            {
                DateCeased = !string.IsNullOrEmpty(dateCeased) ? DateTime.ParseExact(dateCeased, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null,
                InactiveFrom = !string.IsNullOrEmpty(inactiveFrom) ? DateTime.ParseExact(inactiveFrom, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null,
                InactiveTo = !string.IsNullOrEmpty(inactiveTo) ? DateTime.ParseExact(inactiveTo, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null,
                NoPaymentForPeriod = noPaymentForPeriod
            };

            var result = AccountHelper.GetLevyDeclarationDescription(levyDeclaration);

            result.Should().Be(expected);
        }

        [TestCase("£0.00", 0, null, false, null)]
        [TestCase("£0.00", 0, "01/01/0001", false, null)]
        [TestCase("£0.00", 0, null, false, "01/01/0001")]
        [TestCase("", 0, "07/07/2017", false, null)]
        [TestCase("", 0, null, true, null)]
        [TestCase("", 0, null, false, "07/07/2017")]
        [TestCase("£100.00", 100, null, false, null)]
        [TestCase("£100.00", 100, "01/01/0001", false, null)]
        [TestCase("£100.00", 100, null, false, "01/01/0001")]
        [TestCase("", 100, "07/07/2017", false, null)]
        [TestCase("", 100, null, true, null)]
        [TestCase("", 100, null, false, "07/07/2017")]
        [TestCase("£1,000.00", 1000, null, false, null)]
        [TestCase("£1,000,000.00", 1000000, null, false, null)]
        [TestCase("£1,000.53", 1000.53, null, false, null)]
        public void ShouldReturnYearToDateAmount(string expected, decimal ytdAmount, string dateCeased, bool noPayment, string inactiveFrom)
        {
            var declaration = new Declaration
            {
                LevyDueYearToDate = ytdAmount,
                DateCeased = !string.IsNullOrEmpty(dateCeased) ? DateTime.ParseExact(dateCeased, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null,
                InactiveFrom = !string.IsNullOrEmpty(inactiveFrom) ? DateTime.ParseExact(inactiveFrom, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null,
                NoPaymentForPeriod = noPayment
            };
            var result = AccountHelper.GetYearToDateAmount(declaration);

            result.Should().Be(expected);
        }

        [TestCase("17-18 1 (Apr)", 1)]
        [TestCase("17-18 2 (May)", 2)]
        [TestCase("17-18 3 (Jun)", 3)]
        [TestCase("17-18 4 (Jul)", 4)]
        [TestCase("17-18 5 (Aug)", 5)]
        [TestCase("17-18 6 (Sep)", 6)]
        [TestCase("17-18 7 (Oct)", 7)]
        [TestCase("17-18 8 (Nov)", 8)]
        [TestCase("17-18 9 (Dec)", 9)]
        [TestCase("17-18 10 (Jan)", 10)]
        [TestCase("17-18 11 (Feb)", 11)]
        [TestCase("17-18 12 (Mar)", 12)]
        public void ShouldReturnPayrollDate(string expected, short month)
        {
            var payrollDate = new PayrollPeriod
            {
                Month = month,
                Year = "17-18"
            };
            var result = AccountHelper.GetPayrollDate(payrollDate);

            result.Should().Be(expected);
        }

        [Test]
        public void ShouldReturnEmptyWhenPayrollPeriodIsNull()
        {
            var result = AccountHelper.GetPayrollDate(null);

            result.Should().Be(string.Empty);
        }

        [TestCase("10/12/2017 10:50:55 AM", 10, 12, 2017, 10, 50, 55)]
        [TestCase("10/12/2017 10:50:55 PM", 10, 12, 2017, 22, 50, 55)]
        [TestCase("02/10/2017 09:50:55 AM", 2, 10, 2017, 9, 50, 55)]
        [TestCase("20/05/2017 12:05:00 PM", 20, 5, 2017, 12, 5, 0)]
        [TestCase("20/05/2017 12:05:00 AM", 20, 5, 2017, 00, 5, 0)]
        public void ShouldReturnSubmissionDate(string expected, int day, int month, int year, int hour, int min, int sec)
        {
            var submissionTime = new DateTime(year, month, day, hour, min, sec);
            var result = AccountHelper.GetSubmissionDate(submissionTime);

            result.Should().Be(expected);
        }

        [TestCase(1, "Invitation awaiting response")]
        [TestCase(2, "Active")]
        [TestCase(3, "Invitation expired")]
        [TestCase(4, "")]
        public void ShouldReturnInvitationStatus(int status, string expected)
        {
            InvitationStatus invitationStatus = (InvitationStatus)Enum.ToObject(typeof(InvitationStatus), status);
            var result = AccountHelper.GetTeamMemberStatus(invitationStatus);

            result.Should().Be(expected);
        }
    }
}
