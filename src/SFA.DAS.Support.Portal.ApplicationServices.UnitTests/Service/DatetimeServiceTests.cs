using System;
using System.Globalization;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Service
{
    public class DatetimeServiceTests
    {
        private DatetimeService _sut;

        [SetUp]
        public void Init()
        {
            _sut = new DatetimeService();
        }

        [TestCase("31/03/2017", 2016)]
        [TestCase("01/04/2017", 2017)]
        [TestCase("01/09/2017", 2017)]
        [TestCase("01/12/2017", 2017)]
        [TestCase("01/01/2018", 2017)]
        [TestCase("01/03/2018", 2017)]
        [TestCase("01/04/2018", 2018)]
        [TestCase("01/05/2018", 2018)]
        public void ShouldGetYearFromMonth(string date, int expected)
        {
            var actual = _sut.GetYear(DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture));

            actual.Should().Be(expected);
        }

        [TestCase("31/03/2017", "01/04/2016")]
        [TestCase("01/04/2017", "01/04/2017")]
        [TestCase("01/09/2017", "01/04/2017")]
        [TestCase("01/12/2017", "01/04/2017")]
        [TestCase("01/01/2018", "01/04/2017")]
        [TestCase("31/03/2018", "01/04/2017")]
        [TestCase("01/04/2018", "01/04/2018")]
        [TestCase("01/05/2018", "01/04/2018")]
        public void ShouldGetBeginningOfFinancialYear(string date, string expected)
        {
            var actual = _sut.GetBeginningFinancialYear(DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture));

            actual.Should().Be(DateTime.ParseExact(expected, "dd/MM/yyyy", CultureInfo.InvariantCulture));
        }
    }
}
