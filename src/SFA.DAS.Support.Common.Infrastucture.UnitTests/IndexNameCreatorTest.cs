using System;
using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Common.Infrastucture.UnitTests
{
    [TestFixture]
    public class IndexNameCreatorTest
    {
        [TestCase("local_das_support", SearchCategory.User, "local_das_support-user")]
        [TestCase("test_das_support", SearchCategory.Account, "test_das_support-account")]
        public void CreateIndexesAliasNameTest(string indexName, SearchCategory searchCategory, string expected)
        {
            var _sut = new IndexNameCreator();
            var actual = _sut.CreateIndexesAliasName(indexName, searchCategory);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void CreateNewIndexNameTest()
        {
            var _sut = new IndexNameCreator();
            var actual = _sut.CreateNewIndexName("local_das_support", SearchCategory.User);
            var expected = $"local_das_support-user_{DateTime.UtcNow.ToString("yyyyMMddHHmmss").ToLower()}";
            Assert.AreEqual(expected, actual);
        }
    }
}