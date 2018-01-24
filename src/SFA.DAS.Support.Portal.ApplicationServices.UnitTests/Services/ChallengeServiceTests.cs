using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Support.Portal.ApplicationServices.Services;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services
{
    [TestFixture]
    public class ChallengeServiceTests
    {
        [SetUp]
        public void Init()
        {
            _sut = new ChallengeService();
        }

        private ChallengeService _sut;

        [Test]
        public void ShouldOnlyReturnMinNumberFromPayeSchemes()
        {
            var payeSchemes = new List<PayeSchemeViewModel>
            {
                new PayeSchemeViewModel
                {
                    Ref = "705/E1"
                },
                new PayeSchemeViewModel
                {
                    Ref = "524/R246"
                },
                new PayeSchemeViewModel
                {
                    Ref = "114/RA64770"
                }
            };

            for (var i = 1; i <= 10; i++)
            {
                var result = _sut.GetPayeSchemesCharacters(payeSchemes);
                Assert.IsTrue(result.Count == 2);
                Console.WriteLine($"Random chars are {string.Join(" , ", result)}");
                Assert.IsTrue(result[0] == 1 && result[1] == 2 || result[0] == 2 && result[1] == 3 ||
                              result[0] == 1 && result[1] == 3);
                Thread.Sleep(1000);
            }
        }
    }
}