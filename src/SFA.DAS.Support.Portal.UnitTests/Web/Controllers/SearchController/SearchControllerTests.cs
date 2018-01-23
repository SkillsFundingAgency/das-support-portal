using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Portal.Web.ViewModels;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.SearchController
{
    [TestFixture]
    public sealed class SearchControllerTests
    {
        [SetUp]
        public void Init()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockMediator = new Mock<IMediator>();
        }

        private Portal.Web.Controllers.SearchController _sut;
        private Mock<ILog> _mockLogger;
        private Mock<IMappingService> _mockMappingService;
        private Mock<IMediator> _mockMediator;

        [Test]
        public async Task ShouldReturnValidViewModel()
        {
            var query = new SearchQuery
            {
                SearchTerm = "NHS",
                Page = 1,
                SearchType = SearchCategory.User
            };

            var response = new SearchResponse();

            _mockMediator
                .Setup(x => x.SendAsync(query))
                .Returns(Task.FromResult(response));

            _mockMappingService
                .Setup(x => x.Map<SearchResponse, SearchResultsViewModel>(response))
                .Returns(new SearchResultsViewModel());

            _sut = new Portal.Web.Controllers.SearchController(_mockMappingService.Object, _mockMediator.Object);

            var result = await _sut.Index(query);

            var vr = result as ViewResult;

            vr.Should().NotBeNull();

            var vm = vr.Model as SearchResultsViewModel;
            vm.Should().NotBeNull();
        }
    }
}