using System;
using System.Collections.Generic;
using FluentAssertions;
using HMRC.ESFA.Levy.Api.Types;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.UnitTests.Web.ExtensionHelpers;
using SFA.DAS.Support.Portal.Web.Helpers;
using SFA.DAS.Support.Portal.Web.ViewModels;
using SFA.DAS.Support.Portal.Web.Views.Account;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Views.Account
{
    [TestFixture]
    public class PayeSchemeLevySubmissionsPage : ViewTestBase
    {
        [Test]
        public void ShouldShowAllFieldsWhenEverythingIsOk()
        {
            var accountPayeSchemeLevySubmissionsPage = new _Views_Account_PayeSchemeLevySubmissions_cshtml();

            var dateOfLateSubmission = new DateTime(2017, 05, 01);
            var dateOfLatestSubmission = new DateTime(2017, 04, 19);
            var dateOfUnprocessedSubmission = new DateTime(2017, 04, 18);

            var model = new PayeSchemeLevySubmissionViewModel
            {
                LevyDeclarations = new List<Declaration>
                {
                    new Declaration
                    {
                        SubmissionTime = dateOfLateSubmission,
                        PayrollPeriod = new PayrollPeriod() {Month=5,Year="16-17"},
                        NoPaymentForPeriod = false,
                        LevyDeclarationSubmissionStatus = LevyDeclarationSubmissionStatus.LateSubmission
                    },
                    new Declaration
                    {
                        SubmissionTime = dateOfLatestSubmission,
                        PayrollPeriod = new PayrollPeriod() {Month=5,Year="16-17"},
                        NoPaymentForPeriod = false,
                        LevyDeclarationSubmissionStatus = LevyDeclarationSubmissionStatus.LatestSubmission
                    },
                    new Declaration
                    {
                        SubmissionTime = dateOfUnprocessedSubmission,
                        PayrollPeriod = new PayrollPeriod() {Month=5,Year="16-17"},
                        NoPaymentForPeriod = false
                    }
                },
                Account = new Core.Domain.Model.Account()
            };

            var html = accountPayeSchemeLevySubmissionsPage.RenderAsHtml(model).ToAngleSharp();
            
            GetPartial(html, "#submission-details").Should().Contain("Submission date");
            GetPartial(html, ".late").Should().Be(AccountHelper.GetSubmissionDate(dateOfLateSubmission));
            GetPartial(html, ".strong").Should().Be(AccountHelper.GetSubmissionDate(dateOfLatestSubmission));
            GetPartial(html, ".unprocessed-submission").Should().Be(AccountHelper.GetSubmissionDate(dateOfUnprocessedSubmission));
            GetPartial(html, "#no-declarations-found").Should().BeEmpty();
            GetPartial(html, "#no-account-found").Should().BeEmpty();
            GetPartial(html, "#status-no-declarations-found").Should().BeEmpty();
        }

        [Test]
        public void ShouldShowNoDeclarationsFoundWhenNoDeclarationsArePresent()
        {
            var accountPayeSchemeLevySubmissionsPage = new _Views_Account_PayeSchemeLevySubmissions_cshtml();

            var model = new PayeSchemeLevySubmissionViewModel
            {
                LevyDeclarations = new List<Declaration>(),
                Account = new Core.Domain.Model.Account()
            };

            var html = accountPayeSchemeLevySubmissionsPage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#submission-details").Should().BeEmpty();
            GetPartial(html, "#no-account-found").Should().BeEmpty();
            GetPartial(html, "#no-declarations-found").Should().Be("No declarations found for this PAYE scheme");
            GetPartial(html, "#status-no-declarations-found").Should().BeEmpty();
        }

        [Test]
        public void ShouldShowAccountNotFoundIfStatusIsAccountNotFound()
        {
            var accountPayeSchemeLevySubmissionsPage = new _Views_Account_PayeSchemeLevySubmissions_cshtml();

            var model = new PayeSchemeLevySubmissionViewModel
            {
                Status = AccountLevySubmissionsResponseCodes.AccountNotFound,
                Account = new Core.Domain.Model.Account()
            };

            var html = accountPayeSchemeLevySubmissionsPage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#submission-details").Should().BeEmpty();
            GetPartial(html, "#no-account-found").Should().Be("Couldn't find the account");
            GetPartial(html, "#no-declarations-found").Should().BeEmpty();
            GetPartial(html, "#status-no-declarations-found").Should().BeEmpty();
        }

        [Test]
        public void ShouldShowAddedDateWhenPresent()
        {
            var accountPayeSchemeLevySubmissionsPage = new _Views_Account_PayeSchemeLevySubmissions_cshtml();

            var addedDate = DateTime.Today;
            var model = new PayeSchemeLevySubmissionViewModel
            {
                Status = AccountLevySubmissionsResponseCodes.Success,
                Account = new Core.Domain.Model.Account {PayeSchemes = new List<PayeSchemeViewModel> {new PayeSchemeViewModel {Ref = "123", AddedDate = addedDate} } },
                LevyDeclarations = new List<Declaration>
                {
                    new Declaration {NoPaymentForPeriod = true}
                },
                PayePosition = "0"
            };

            var html = accountPayeSchemeLevySubmissionsPage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".paye-scheme-added-date").Should().Contain(AccountHelper.ConvertDateTimeToDdmmyyyyFormat(addedDate));
        }

        [Test]
        public void ShouldShowName()
        {
            var accountPayeSchemeLevySubmissionsPage = new _Views_Account_PayeSchemeLevySubmissions_cshtml();

            var name = "NAME";

            var model = new PayeSchemeLevySubmissionViewModel
            {
                Status = AccountLevySubmissionsResponseCodes.Success,
                Account = new Core.Domain.Model.Account { PayeSchemes = new List<PayeSchemeViewModel> { new PayeSchemeViewModel { Ref = "123", Name = name} } },
                LevyDeclarations = new List<Declaration>
                {
                    new Declaration {NoPaymentForPeriod = true}
                },
                PayePosition = "0"
            };

            var html = accountPayeSchemeLevySubmissionsPage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".paye-scheme-name").Should().Contain(name);
        }

        [Test]
        public void ShouldNotShowAddedDateWhenNotPresent()
        {
            var accountPayeSchemeLevySubmissionsPage = new _Views_Account_PayeSchemeLevySubmissions_cshtml();

            var model = new PayeSchemeLevySubmissionViewModel
            {
                Status = AccountLevySubmissionsResponseCodes.Success,
                Account = new Core.Domain.Model.Account { PayeSchemes = new List<PayeSchemeViewModel> { new PayeSchemeViewModel { Ref = "123", AddedDate = DateTime.MinValue} } },
                LevyDeclarations = new List<Declaration>
                {
                    new Declaration {NoPaymentForPeriod = true}
                },
                PayePosition = "0"
            };

            var html = accountPayeSchemeLevySubmissionsPage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".paye-scheme-added-date").Should().BeEmpty();
        }

        [Test]
        public void ShouldShowAccountNotFoundIfStatusIsDeclarationsNotFound()
        {
            var accountPayeSchemeLevySubmissionsPage = new _Views_Account_PayeSchemeLevySubmissions_cshtml();

            var model = new PayeSchemeLevySubmissionViewModel
            {
                Status = AccountLevySubmissionsResponseCodes.DeclarationsNotFound,
                Account = new Core.Domain.Model.Account()
            };

            var html = accountPayeSchemeLevySubmissionsPage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#submission-details").Should().BeEmpty();
            GetPartial(html, "#status-no-declarations-found").Should().Be("No declarations found for this PAYE scheme");
            GetPartial(html, "#no-declarations-found").Should().BeEmpty();
            GetPartial(html, "#no-account-found").Should().BeEmpty();

        }
    }
}
