using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Core.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Web.ViewModels;
using AccountDetailViewModel = SFA.DAS.EAS.Account.Api.Types.AccountDetailViewModel;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.SearchController
{
    [TestFixture]
    public sealed class WhenCallingIndexGet : WhenTestingSearchController
    {
        [Test]
        public async Task ItShouldReturnADefaultViewWhenTheSearchTermIsEmpty()
        {
            var query = new EmployerUserSearchQuery {SearchTerm = string.Empty};

            var result = await Unit.Index(query);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var view = (ViewResult) result;

            Assert.IsInstanceOf<SearchResultsViewModel>(view.Model);

            var model = (SearchResultsViewModel) view.Model;

            CollectionAssert.IsEmpty(model.Results);
        }

        [Test]
        public async Task ItShouldReturnADefaultViewWhenTheSearchTermIsNull()
        {
            var query = new EmployerUserSearchQuery {SearchTerm = null};

            var result = await Unit.Index(query);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var view = (ViewResult) result;

            Assert.IsInstanceOf<SearchResultsViewModel>(view.Model);

            var model = (SearchResultsViewModel) view.Model;

            CollectionAssert.IsEmpty(model.Results);
        }

        [Test]
        public async Task ItShouldReturnADefaultViewWhenTheSearchTermIsWhiteSpace()
        {
            var query = new EmployerUserSearchQuery {SearchTerm = " "};

            var result = await Unit.Index(query);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var view = (ViewResult) result;

            Assert.IsInstanceOf<SearchResultsViewModel>(view.Model);

            var model = (SearchResultsViewModel) view.Model;

            CollectionAssert.IsEmpty(model.Results);
        }

        [Test]
        public async Task ItShouldReturnTheV2BehaviourWhenTheNewHeaderIsFound()
        {
            var query = new EmployerUserSearchQuery { SearchTerm = "find me something nice" };

            var configurationAttributeCollection = new ConfigurationAttributeCollection
            {
                new NameValueCollection
                {
                    {"new", ""}
                }
            };

            MockRequestBase.Setup(x => x.Headers).Returns(configurationAttributeCollection);

         
            var mediatorResponse = new SearchResponse()
            {
                Results = new List<string>() {  "Some", "results"}
            };    


            MockMediator.Setup(x=>x.SendAsync(It.IsAny<SearchQuery>()))
                .ReturnsAsync(mediatorResponse);
            


            var result = await Unit.Index(query);

            Assert.IsNotNull(result);

            Assert.IsInstanceOf<ViewResult>(result);

            var view = (ViewResult) result;

            Assert.IsInstanceOf<SearchResultsViewModel>(view.Model);

            var model = (SearchResultsViewModel) view.Model;

            Assert.AreEqual(query.SearchTerm, model.SearchTerm);
            CollectionAssert.IsEmpty(model.Results);
            CollectionAssert.IsNotEmpty(model.NewResults);


        }


        [Test]
        public async Task ItShouldReturnTheV1ViewWhenTheNewHeaderIsNotFoundAndTheSearchIsSuccessful()
        {
            var query = new EmployerUserSearchQuery { SearchTerm = "find me something nice" };

            var configurationAttributeCollection = new ConfigurationAttributeCollection
            {
                new NameValueCollection
                {
                  // No special headers
                }
            };

            MockRequestBase.Setup(x => x.Headers).Returns(configurationAttributeCollection);


            var mediatorResponse = new EmployerUserSearchResponse()
            {
                StatusCode = SearchResponseCodes.Success,
                Results = new List<UserSummary>() { new UserSummary()
                {
                    Email = "some.one@tempuri.org",
                    FirstName = "Some", LastName = "one", Href = "https://tempuri.org", Id = "123", Status = UserStatus.Active, Accounts = new List<AccountDetailViewModel>(){}
                } },
                Page = 1,
                SearchTerm = query.SearchTerm,
                LastPage = 1
            };


            MockMediator.Setup(x => x.SendAsync(query))
                .ReturnsAsync(mediatorResponse);


            var viewModel = new SearchResultsViewModel()
            {
                Results = mediatorResponse.Results // mapper only maps this property
            };

            MockMappingService.Setup(x =>
                x.Map<EmployerUserSearchResponse, SearchResultsViewModel>(mediatorResponse))
                .Returns(viewModel);



            var result = await Unit.Index(query);

            Assert.IsNotNull(result);

            Assert.IsInstanceOf<ViewResult>(result);

            var view = (ViewResult)result;

            Assert.IsInstanceOf<SearchResultsViewModel>(view.Model);

            var model = (SearchResultsViewModel)view.Model;

            Assert.IsNull(model.SearchTerm); 
            Assert.IsNull(model.ErrorMessage);
            CollectionAssert.IsNotEmpty(model.Results);


        }



        [Test]
        public async Task ItShouldReturnTheV1ViewWithErrorMessageWhenTheNewHeaderIsNotFoundAndTheSearchIsNotSuccessful()
        {
            var query = new EmployerUserSearchQuery { SearchTerm = "find me something nice" };

            var configurationAttributeCollection = new ConfigurationAttributeCollection
            {
                new NameValueCollection
                {
                  // No special headers
                }
            };

            MockRequestBase.Setup(x => x.Headers).Returns(configurationAttributeCollection);


            var mediatorResponse = new EmployerUserSearchResponse()
            {
                StatusCode = SearchResponseCodes.SearchFailed,
                Page = 1,
                SearchTerm = query.SearchTerm,
                LastPage = 1
            };


            MockMediator.Setup(x => x.SendAsync(query))
                .ReturnsAsync(mediatorResponse);


            
            var result = await Unit.Index(query);



            MockMappingService.Verify(x =>
                    x.Map<EmployerUserSearchResponse, SearchResultsViewModel>(mediatorResponse), Times.Never );
                


            Assert.IsNotNull(result);

            Assert.IsInstanceOf<ViewResult>(result);

            var view = (ViewResult)result;

            Assert.IsInstanceOf<SearchResultsViewModel>(view.Model);

            var model = (SearchResultsViewModel)view.Model;

            Assert.IsNull(model.SearchTerm);
            Assert.IsNotNull(model.ErrorMessage);
            CollectionAssert.IsEmpty(model.Results);


        }

    }
}