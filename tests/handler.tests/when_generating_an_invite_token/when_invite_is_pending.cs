using System;
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
    public class when_invite_is_pending : InMemoryContext<GameTrackerContext>
    {
        private GenerateInviteTokenHandler _subject;
        private Mock<ITokenService> _tokenService;
        private string _result;
        private string _expectedToken = "previoustoken";
        private readonly Guid _tenantId = new Guid("70E7CB58-B7C2-4BC9-9D53-3ED76AEA977B");

        public when_invite_is_pending()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _tokenService = new Mock<ITokenService>();

            _tokenService.Setup(ts => ts.TokenFromGuid(It.IsAny<Guid>())).Returns("novalidtoken");

            _subject = new GenerateInviteTokenHandler(Context, _tokenService.Object);

            Context.TenantInvites.Add(new TenantInvite
            {
                TenantId = _tenantId,
                Token = _expectedToken,
                Expiration = DateTime.UtcNow.AddDays(1)
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _result = _subject.Handle(new GenerateInviteToken
            {
                TenantId = _tenantId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void previously_generated_token_is_returned()
        {
            _result.Should().Be(_expectedToken);
        }
    }
}