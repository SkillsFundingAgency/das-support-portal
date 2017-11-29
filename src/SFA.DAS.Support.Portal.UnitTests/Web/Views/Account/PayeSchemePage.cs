using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Support.Portal.UnitTests.Web.ExtensionHelpers;
using SFA.DAS.Support.Portal.Web.Helpers;
using SFA.DAS.Support.Portal.Web.Views.Account;
using AccountDetailViewModel = SFA.DAS.Support.Portal.Web.ViewModels.AccountDetailViewModel;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Views.Account
{
    [TestFixture]
    public class PayeSchemePage : ViewTestBase
    {
        [Test]
        [TestCaseSource(nameof(GetEmptyPayeSchemes))]
        public void ShouldNotShowPayeSchemesTable(IEnumerable<PayeSchemeViewModel> payeSchemes)
        {
            var payeSchemePage = new _Views_Account_PayeSchemes_cshtml();

            var model = new AccountDetailViewModel
            {
                Account = new Core.Domain.Model.Account { PayeSchemes = payeSchemes}
            };

            var html = payeSchemePage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#no-paye-schemes-found").Should().NotBeEmpty();
        }

        [Test]
        public void ShouldShowPayeRef()
        {
            var payeSchemePage = new _Views_Account_PayeSchemes_cshtml();

            var payeRef = "TEST";

            var model = new AccountDetailViewModel
            {
                Account = new Core.Domain.Model.Account { PayeSchemes = new List<PayeSchemeViewModel> {new PayeSchemeViewModel {Ref = payeRef} } }
            };

            var html = payeSchemePage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#paye-ref").Should().Contain(payeRef);
        }

        [Test]
        public void ShouldShowPayeName()
        {
            var payeSchemePage = new _Views_Account_PayeSchemes_cshtml();

            var name = "TEST";

            var model = new AccountDetailViewModel
            {
                Account = new Core.Domain.Model.Account { PayeSchemes = new List<PayeSchemeViewModel> { new PayeSchemeViewModel { Name = name } } }
            };

            var html = payeSchemePage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#paye-scheme-name").Should().Contain(name);
        }

        [Test]
        public void ShouldShowAccountNotFoundWhenNoAccountExists()
        {
            var payeSchemePage = new _Views_Account_PayeSchemes_cshtml();

            var model = new AccountDetailViewModel
            {
                Account = null
            };

            var html = payeSchemePage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#no-account-found").Should().NotBeEmpty();
        }

        [Test]
        public void ShouldShowAddedDate()
        {
            var payeSchemePage = new _Views_Account_PayeSchemes_cshtml();

            var addedDate = DateTime.Today;

            var model = new AccountDetailViewModel
            {
                Account = new Core.Domain.Model.Account { PayeSchemes = new List<PayeSchemeViewModel> { new PayeSchemeViewModel { AddedDate = addedDate } } }
            };

            var html = payeSchemePage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#paye-scheme-added-date").Should().Contain(AccountHelper.ConvertDateTimeToDdmmyyyyFormat(addedDate));
        }


        private static IEnumerable<IEnumerable<PayeSchemeViewModel>> GetEmptyPayeSchemes()
        {
            yield return null;
            yield return new List<PayeSchemeViewModel>();
        }
    }
}