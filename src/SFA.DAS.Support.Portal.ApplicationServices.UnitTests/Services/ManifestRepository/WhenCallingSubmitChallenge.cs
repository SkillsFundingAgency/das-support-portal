using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingSubmitChallenge : WhenTestingManifestRepository
    {

        private string _id;
        private Dictionary<string, string> _submittedFormData;
        private string _uriString;
        private string _redirectUrl;
        private string _innerAction;
        private string _challengekey;
        private IDictionary<string, string> _postedFormData;
        private string _resourceKey;


        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _id = "123";
            _uriString = "https://tempuri.org";
            _resourceKey = SupportServiceResourceKey.EmployerAccountFinance.ToString();
            _challengekey = SupportServiceResourceKey.EmployerAccountFinanceChallenge.ToString();
            _innerAction = $"/api/challenge/{_id}";
            _redirectUrl = $"{_uriString}/redirect/{_id}";

            _submittedFormData = new Dictionary<string, string>
            {
                {"redirect", _redirectUrl},
                {"innerAction", _innerAction},
                {"resourceKey", _resourceKey},
                {"challengeKey", _challengekey},
                {"Key1", "Value1"},
                {"Key2", "Value2"}
            };

            _postedFormData = new Dictionary<string, string>();

            foreach (var item in _submittedFormData)
            {
                if (new[] { "redirect", "innerAction", "challengeKey" }.Contains(item.Key)) continue;
                _postedFormData.Add(item.Key, item.Value);
            }
        }

        [Test]
        public async Task ItShouldReceiveAFormResponseOnFail()
        {
            var mappedFormHtml = "<form action='/api/challenge/id'  method='post' />";

            MockFormMapper.Setup(x => x.UpdateForm(
                    It.IsAny<SupportServiceResourceKey>(),
                    It.IsAny<SupportServiceResourceKey>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
                .Returns(mappedFormHtml);


            MockSiteConnector
                .Setup(x => x.Upload<string>(It.IsAny<Uri>(), It.IsAny<IDictionary<string, string>>()))
                .ReturnsAsync(mappedFormHtml);

            MockSiteConnector.SetupGet(x => x.LastCode).Returns(HttpStatusCode.OK);

            var result = await Unit.SubmitChallenge(_id, _submittedFormData);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasRedirect);
            Assert.IsNull(result.RedirectUrl);
            Assert.AreEqual(mappedFormHtml, result.Page);
        }

        [Test]
        public async Task ItShouldReceiveJustARedirectOnSuccess()
        {
            var challengeResult = new ChallengeValidationResult
            {
                IsValidResponse = true
            };

            MockSiteConnector
                .Setup(x => x.Upload<string>(It.IsAny<Uri>(), It.IsAny<IDictionary<string, string>>()))
                .Returns(Task.FromResult(JsonConvert.SerializeObject(challengeResult)));

            var result = await Unit.SubmitChallenge(_id, _submittedFormData);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasRedirect);
            Assert.AreEqual(_redirectUrl, result.RedirectUrl);
            Assert.IsNull(result.Page);
        }


        [Test]
        public void ItShouldThrowAnExceptionIfTheFormChallengeKeyIsInvalid()
        {
            //MissingMemberException
            _submittedFormData = new Dictionary<string, string>
            {
                {"redirect", _redirectUrl},
                {"innerAction", _innerAction},
                {"resourceKey", _resourceKey.ToString()},
                {"challengeKey", "BadChallengKey"},
                {"Key1", "Value1"},
                {"Key2", "Value2"}
            };

            Assert.ThrowsAsync<ArgumentException>(() => Unit.SubmitChallenge(_id, _submittedFormData));
        }

        /// <summary>
        ///     See https://skillsfundingagency.atlassian.net/browse/ASCS-88
        ///     For what to do about this and other issues in the codebase
        /// </summary>
        /// <returns></returns>
        [Test]
        public void ItShouldThrownAnExceptionIfTheSubmissionErrors()
        {
            var httpException = new HttpException();
            MockSiteConnector
                .Setup(x => x.Upload<string>(It.IsAny<Uri>(), _postedFormData))
                .ThrowsAsync(httpException);

            MockSiteConnector.SetupGet(x => x.LastCode).Returns(HttpStatusCode.InternalServerError);
            MockSiteConnector.SetupGet(x => x.LastException).Returns(httpException);


            Assert.ThrowsAsync<HttpException>(() => Unit.SubmitChallenge(_id, _submittedFormData));
        }
    }
}