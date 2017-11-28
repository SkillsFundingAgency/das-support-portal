using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HMRC.ESFA.Levy.Api.Types;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Handlers;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Domain.Exceptions;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Core.Helpers;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    public class AccountLevySubmissionsHandlerTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockLevySubmissionsRepository = new Mock<ILevySubmissionsRepository>();
            _mockPayeSchemeObfuscator = new Mock<IPayeSchemeObfuscator>();
            _mockLogger = new Mock<ILog>();


            _sut = new AccountLevySubmissionsHandler(_mockAccountRepository.Object, _mockLevySubmissionsRepository.Object,
                _mockPayeSchemeObfuscator.Object, _mockLogger.Object);
        }

        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<ILevySubmissionsRepository> _mockLevySubmissionsRepository;
        private Mock<IPayeSchemeObfuscator> _mockPayeSchemeObfuscator;
        private Mock<ILog> _mockLogger;
        private AccountLevySubmissionsHandler _sut;

        [Test]
        public void ShouldReturnAccountNotFoundIfNoAccountFound()
        {
            const string id = "1";
            const string position = "4";
            var message = new AccountLevySubmissionsQuery(id, position);
            _mockAccountRepository.Setup(x => x.Get(id, It.IsAny<AccountFieldsSelection>())).Returns(Task.FromResult((Account) null));
            var actual = _sut.Handle(message);
            actual.Result.StatusCode.Should().Be(AccountLevySubmissionsResponseCodes.AccountNotFound);
        }

        [Test]
        public void ShouldReturnAddedDate()
        {
            const string id = "16";
            const string position = "0";

            var addedDate = DateTime.Today;

            var message = new AccountLevySubmissionsQuery(id, position);

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel {AddedDate = addedDate}
                }
            };

            _mockAccountRepository.Setup(x => x.Get(id, It.IsAny<AccountFieldsSelection>())).Returns(Task.FromResult(account));

            var actual = _sut.Handle(message);

            Assert.AreEqual(addedDate, actual.Result.Account.PayeSchemes.ToList()[0].AddedDate);
        }

        [Test]
        public void ShouldReturnName()
        {
            const string id = "16";
            const string position = "0";

            var name = "NAME";

            var message = new AccountLevySubmissionsQuery(id, position);

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel {Name = name}
                }
            };

            _mockAccountRepository.Setup(x => x.Get(id, It.IsAny<AccountFieldsSelection>())).Returns(Task.FromResult(account));

            var actual = _sut.Handle(message);

            Assert.AreEqual(name, actual.Result.Account.PayeSchemes.ToList()[0].Name);
        }

        [Test]
        public void ShouldReturnNotFound()
        {
            const string id = "1";
            const string position = "2";
            var message = new AccountLevySubmissionsQuery(id, position);
            const string selectedPayeSchemeRef = "ref 2";

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel {Ref = "ref 0"},
                    new PayeSchemeViewModel {Ref = "ref 1"},
                    new PayeSchemeViewModel {Ref = selectedPayeSchemeRef, AddedDate = new DateTime(2017, 01, 01), RemovedDate = null, Name = "name 2", DasAccountId = "12345"},
                    new PayeSchemeViewModel {Ref = "ref 3"}
                }
            };

            _mockAccountRepository.Setup(x => x.Get(id, It.IsAny<AccountFieldsSelection>())).Returns(Task.FromResult(account));
            _mockLevySubmissionsRepository.Setup(x => x.Get(selectedPayeSchemeRef)).Throws(new EntityNotFoundException("error message", selectedPayeSchemeRef, null));
            var actual = _sut.Handle(message);

            actual.Result.StatusCode.Should().Be(AccountLevySubmissionsResponseCodes.DeclarationsNotFound);
        }

        [Test]
        public void ShouldReturnSuccessIfAccountFoundAndLevySubmissionsExist()
        {
            const string id = "1";
            const string position = "2";
            var message = new AccountLevySubmissionsQuery(id, position);
            const string selectedPayeSchemeRef = "ref 2";

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel {Ref = "ref 0"},
                    new PayeSchemeViewModel {Ref = "ref 1"},
                    new PayeSchemeViewModel {Ref = selectedPayeSchemeRef, AddedDate = new DateTime(2017, 01, 01), RemovedDate = null, Name = "name 2", DasAccountId = "12345"},
                    new PayeSchemeViewModel {Ref = "ref 3"}
                }
            };

            _mockAccountRepository.Setup(x => x.Get(id, It.IsAny<AccountFieldsSelection>())).Returns(Task.FromResult(account));
            _mockLevySubmissionsRepository.Setup(x => x.Get(selectedPayeSchemeRef)).Returns(Task.FromResult(new LevyDeclarations()));


            var actual = _sut.Handle(message);

            actual.Result.StatusCode.Should().Be(AccountLevySubmissionsResponseCodes.Success);
        }


        [Test]
        public void ShouldReturnSuccessIfAccountFoundAndNoLevySubmissionsExist()
        {
            const string id = "1";
            const string position = "2";
            var message = new AccountLevySubmissionsQuery(id, position);
            const string selectedPayeSchemeRef = "ref 2";

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel {Ref = "ref 0"},
                    new PayeSchemeViewModel {Ref = "ref 1"},
                    new PayeSchemeViewModel {Ref = selectedPayeSchemeRef, AddedDate = new DateTime(2017, 01, 01), RemovedDate = null, Name = "name 2", DasAccountId = "12345"},
                    new PayeSchemeViewModel {Ref = "ref 3"}
                }
            };

            _mockAccountRepository.Setup(x => x.Get(id, It.IsAny<AccountFieldsSelection>())).Returns(Task.FromResult(account));
            _mockLevySubmissionsRepository.Setup(x => x.Get(selectedPayeSchemeRef)).Returns(Task.FromResult((LevyDeclarations) null));
            var actual = _sut.Handle(message);

            actual.Result.StatusCode.Should().Be(AccountLevySubmissionsResponseCodes.Success);
        }

        [Test]
        public void ShouldThrowException()
        {
            const string id = "1";
            const string position = "2";
            var message = new AccountLevySubmissionsQuery(id, position);
            const string selectedPayeSchemeRef = "ref 2";

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel {Ref = "ref 0"},
                    new PayeSchemeViewModel {Ref = "ref 1"},
                    new PayeSchemeViewModel {Ref = selectedPayeSchemeRef, AddedDate = new DateTime(2017, 01, 01), RemovedDate = null, Name = "name 2", DasAccountId = "12345"},
                    new PayeSchemeViewModel {Ref = "ref 3"}
                }
            };

            _mockAccountRepository.Setup(x => x.Get(id, It.IsAny<AccountFieldsSelection>())).Returns(Task.FromResult(account));
            _mockLevySubmissionsRepository.Setup(x => x.Get(selectedPayeSchemeRef)).Throws(new Exception());

            var actual = _sut.Handle(message);

            actual.Exception.Should().NotBe(null);

            actual.Exception.GetBaseException().Should().BeOfType<Exception>();
        }
    }
}