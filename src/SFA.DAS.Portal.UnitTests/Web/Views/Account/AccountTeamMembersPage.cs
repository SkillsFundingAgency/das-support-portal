using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Portal.UnitTests.Web.ExtensionHelpers;
using SFA.DAS.Portal.Web.Views.Account;
using AccountDetailViewModel = SFA.DAS.Portal.Web.ViewModels.AccountDetailViewModel;

namespace SFA.DAS.Portal.UnitTests.Web.Views.Account
{
    [TestFixture]
    public class AccountTeamMembersPage : ViewTestBase
    {
        [Test]
        public void ShouldShowAllFieldsWhenEverythingIsOk()
        {
            var teamMembersPage = new _Views_Account_TeamMembers_cshtml();

            var model = new AccountDetailViewModel
            {
                SearchUrl = "www.abba.co.uk",
                Account = new Core.Domain.Model.Account
                {
                    HashedAccountId = "PAPAYA",
                    DasAccountName = "AY CARAMBA",
                    TeamMembers = new List<TeamMemberViewModel>
                    {
                        new TeamMemberViewModel
                        {
                            Name = "Test user 1",
                            UserRef = "TestUserRef1",
                            Email = "testUserEmail1",
                            Role = "Owner"
                        },
                        new TeamMemberViewModel
                        {
                            Name = "Test user 2",
                            UserRef = "",
                            Email = "testUserEmail2",
                            Role = "Viewer"
                        },
                        new TeamMemberViewModel
                        {
                            Name = "Test user 3",
                            UserRef = "",
                            Email = "testUserEmail3",
                            Role = "Viewer"
                        }
                    }
                }
            };
            
            var html = teamMembersPage.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "#teamMemberName").Should().Be("Test user 1");
            this.GetPartial(html, "#teamMemberName", 2).Should().Be("Test user 2");
            this.GetPartial(html, "#teamMemberName", 3).Should().Be("Test user 3");

            this.GetPartial(html, "#teamMemberEmail").Should().Be("testUserEmail1");
            this.GetPartial(html, "#teamMemberEmail", 2).Should().Contain("testUserEmail2");
            this.GetPartial(html, "#teamMemberEmail", 3).Should().Contain("testUserEmail3");
        }
    }
}
