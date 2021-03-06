﻿using System;
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
    public class when_invite_exists_but_has_expired : InMemoryContext<GameTrackerContext>
    {
        private GenerateInviteTokenHandler _subject;
        private Mock<ITokenService> _tokenService;
        private string _result;
        private string _email = "email@domain.com";
        private string _expectedToken = "newtoken";
        private readonly Guid _tenantId = new Guid("70E7CB58-B7C2-4BC9-9D53-3ED76AEA977B");

        public when_invite_exists_but_has_expired()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _tokenService = new Mock<ITokenService>();

            _tokenService.Setup(ts => ts.TokenFromGuid(It.IsAny<Guid>())).Returns(_expectedToken);

            _subject = new GenerateInviteTokenHandler(Context, _tokenService.Object);

            Context.TenantInvites.Add(new TenantInvite
            {
                TenantId = _tenantId,
                Token = "expiredtoken",
                Expiration = DateTime.UtcNow.AddDays(-1)
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
        public void new_token_is_returned()
        {
            _result.Should().Be(_expectedToken);
        }

        [Fact]
        public void expired_token_is_removed()
        {
            Context.TenantInvites.Count(ti => ti.TenantId == _tenantId).Should().Be(1);
        }
    }
}