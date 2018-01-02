using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Shared.SearchIndexModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Common.Infrastucture.Indexer.Tests
{
    [TestFixture()]
    public class IndexNameCreatorTest
    {

        [TestCase("{0}_das_support", "Local",SearchCategory.User, "local_das_support-user")]
        [TestCase("{0}_das_support", "Test", SearchCategory.Account, "test_das_support-account")]
        public void CreateIndexesAliasNameTest(string IndexNameFormat, string environment, SearchCategory searchCategory, string expected)
        {
            var _sut = new IndexNameCreator();
            var actual =_sut.CreateIndexesAliasName(IndexNameFormat, environment, searchCategory);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void CreateIndexesToDeleteNameTest()
        {
            var _sut = new IndexNameCreator();
            var actual = _sut.CreateIndexesToDeleteName("{0}_das_support", "Local", SearchCategory.User);
            var expected = $"local_das_support-user_{DateTime.UtcNow.AddDays(-1).ToString("yyyy-MMM-dd").ToLower()}";
            Assert.AreEqual(actual, expected);
        }

        [Test()]
        public void CreateNewIndexNameTest()
        {
            var _sut = new IndexNameCreator();
            var actual = _sut.CreateNewIndexName("{0}_das_support", "Local", SearchCategory.User);
            var expected = $"local_das_support-user_{DateTime.UtcNow.ToString("yyyy-MMM-dd-HH-mm").ToLower()}";
            Assert.AreEqual(actual, expected);
        }
    }
}