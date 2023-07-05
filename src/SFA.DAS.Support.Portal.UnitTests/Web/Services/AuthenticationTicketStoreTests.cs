using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Web.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Serializer;
using SFA.DAS.Support.Portal.Web.Interfaces;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Services
{
    public class AuthenticationTicketStoreTests
    {
        public const string AuthenticationScheme = "Test";

        [Test, MoqAutoData]
        public async Task Then_The_Ticket_Is_Added_To_The_Store(
            int expiryTime,
            [Frozen] Mock<IDistributedCache> distributedCache,
            [Frozen] Mock<IDfESignInServiceConfiguration> config,
            AuthenticationTicketStore authenticationTicketStore)
        {
            //arrange 
            var ticket = GetMockAuthenticationTicket;

            config.Object.DfEOidcConfiguration.LoginSlidingExpiryTimeOutInMinutes = expiryTime;

            var result = await authenticationTicketStore.StoreAsync(ticket);

            Assert.That(Guid.TryParse(result, out var actualKey), Is.True);
            distributedCache.Verify(x => x.SetAsync(
                actualKey.ToString(),
                It.IsAny<byte[]>(),
                It.Is<DistributedCacheEntryOptions>(c
                    => c.SlidingExpiration != null && c.SlidingExpiration.Value.Minutes == TimeSpan.FromMinutes(expiryTime).Minutes),
                It.IsAny<CancellationToken>()
                ), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Ticket_Is_Retrieved_By_Id_From_The_Store(
            string key,
            [Frozen] Mock<IDistributedCache> distributedCache,
            AuthenticationTicketStore authenticationTicketStore)
        {
            //arrange 
            var ticket = GetMockAuthenticationTicket;

            distributedCache.Setup(x => x.GetAsync(key, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TicketSerializer().Serialize(ticket));

            var result = await authenticationTicketStore.RetrieveAsync(key);

            result.Should().NotBeNull();
            result.Should().BeOfType<AuthenticationTicket>();
        }

        [Test, MoqAutoData]
        public async Task Then_Null_Is_Returned_If_The_Key_Does_Not_Exist(
            string key,
            [Frozen] Mock<IDistributedCache> distributedCache,
            AuthenticationTicketStore authenticationTicketStore)
        {
            distributedCache.Setup(x => x.GetAsync(key, It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[])null);

            var result = await authenticationTicketStore.RetrieveAsync(key);

            result.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Key_Is_Refreshed(
            string key,
            [Frozen] Mock<IDistributedCache> distributedCache,
            AuthenticationTicketStore authenticationTicketStore)
        {
            //arrange 
            var ticket = GetMockAuthenticationTicket;

            await authenticationTicketStore.RenewAsync(key, ticket);

            distributedCache.Verify(x => x.RefreshAsync(key, CancellationToken.None));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Key_Is_Removed(
            string key,
            [Frozen] Mock<IDistributedCache> distributedCache,
            AuthenticationTicketStore authenticationTicketStore)
        {
            await authenticationTicketStore.RemoveAsync(key);

            distributedCache.Verify(x => x.RemoveAsync(key, CancellationToken.None));
        }

        private static AuthenticationTicket GetMockAuthenticationTicket =>
            new AuthenticationTicket(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "Test user")
                    }, AuthenticationScheme), 
                new AuthenticationProperties());
        
    }
}
