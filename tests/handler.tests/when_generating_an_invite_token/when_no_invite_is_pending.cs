using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.Services;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using Moq;
using Xunit;

namespace handler.tests.when_generating_an_invite_token.when_user_exists
{
    public class when_no_invite_is_pending : InMemoryContext<GameTrackerContext>
    {
        private GenerateInviteTokenHandler _subject;
        private Mock<ITokenService> _tokenService;
        private string _result;
        private string _email = "email@domain.com";
        private string _expectedToken = "invitetoken";
        private readonly Guid _tenantId = new Guid("70E7CB58-B7C2-4BC9-9D53-3ED76AEA977B");

        public when_no_invite_is_pending()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _tokenService = new Mock<ITokenService>();

            _tokenService.Setup(ts => ts.TokenFromGuid(It.IsAny<Guid>())).Returns(_expectedToken);

            _subject = new GenerateInviteTokenHandler(Context, _tokenService.Object);
        }

        private void Act()
        {
            _result = _subject.Handle(new GenerateInviteToken
            {
                TenantId = _tenantId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void token_service_is_invoked_with_tenant_id()
        {
            _tokenService.Verify(ts => ts.TokenFromGuid(It.Is<Guid>(g => g == _tenantId)), Times.Once);
        }

        [Fact]
        public void invite_is_added()
        {
            var invite =
                Context.TenantInvites.SingleOrDefault(ti => ti.TenantId == _tenantId);

            invite.Should().NotBeNull();
            invite.Token.Should().Be(_expectedToken);
            invite.Expiration.Should().BeWithin(TimeSpan.FromHours(4)).After(DateTime.UtcNow);
        }

        [Fact]
        public void token_is_returned()
        {
            _result.Should().Be(_expectedToken);
        }
    }
}