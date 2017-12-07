using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Handlers;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    public class SearchHandlerTests
    {
        private SearchHandler _unit;
        private Mock<IEntityRepository> _mockEntityRepository;
        private SearchResponse _actual;

        [SetUp]
        public void Setup()
        {
            _mockEntityRepository = new Mock<IEntityRepository>();
            _unit = new SearchHandler(_mockEntityRepository.Object);
        }

        [Test]
        public async Task ItShouldReturnTheRepositoryResultsForTheQuery()
        {
            
            var searchQuery = new SearchQuery(){ Query= "something"};
            var expected = new List<string>(){"somehit", "anotherhit"}; 
            
            _mockEntityRepository.Setup(x => x.Search(searchQuery.Query))
                .Returns(expected);

            _actual = await _unit.Handle(searchQuery);

           CollectionAssert.IsNotEmpty(_actual.Results);
            Assert.AreEqual(expected.Count, _actual.Results.ToList().Count);
        }

    }
}