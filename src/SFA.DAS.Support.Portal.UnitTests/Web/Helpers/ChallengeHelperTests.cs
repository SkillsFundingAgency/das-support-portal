using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Web.Helpers;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Helpers
{
    [TestFixture]
    public sealed class ChallengeHelperTests
    {
        [TestCase("2nd & 3rd", 1, 2)]
        [TestCase("3rd & 4th", 2, 3)]
        [TestCase("4th & 5th", 3, 4)]
        [TestCase("11th & 12th", 10, 11)]
        [TestCase("21st & 22nd", 20, 21)]
        public void ShouldReturnCharacterChallengeMessage(string expected, params int[] challengePositions)
        {
            var result = ChallengeHelper.GetChallengeMessage(challengePositions.ToList());

            result.Should().NotBeEmpty();
            result.Should().StartWith(expected);
        }
    }
}
