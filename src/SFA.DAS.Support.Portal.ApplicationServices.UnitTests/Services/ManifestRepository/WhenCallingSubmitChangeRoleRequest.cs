using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    public class WhenCallingSubmitChangeRoleRequest : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShouldUploadAnUpdateRoleRequest()
        {
            const string hashedAccountId = "BADWASSD";
            var userRef = Guid.NewGuid().ToString();
            const string role = "TEST";
            const string supportEmail = "support@email.test";

            var url = new Uri($"{BaseUrl}roles/{hashedAccountId}/{userRef}");

            var jsonRequest = JsonConvert.SerializeObject(new UpdateRoleRequest
            {
                Role = role,
                SupportUserEmail = supportEmail
            });

            var resourceIdentity = MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri;
            
            MockSiteConnector.Setup(x => x.Upload(url, jsonRequest, resourceIdentity)).Returns(Task.CompletedTask);

            await Unit.SubmitChangeRoleRequest(hashedAccountId, userRef, role, supportEmail);

            MockSiteConnector.Verify(x => x.Upload(url, jsonRequest, resourceIdentity), Times.Once);
        }
    }
}