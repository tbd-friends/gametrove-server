using System;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using Xunit;

namespace handler.tests.when_registering_user.when_token_provided
{
    public class when_invite_has_expired_and_user_not_found : InMemoryContext<GameTrackerContext>
    {
        private RegisterUserHandler _subject;
        private string _email = "EmailAddress";
        private string _identifier = "Identifier";
        private string _inviteToken = "Invitation";
        private readonly Guid _existingTenantId = new Guid("104128AB-68E9-4BF8-A223-5B04DC085292");

        public when_invite_has_expired_and_user_not_found()
        {
            Arrange();
        }

        private void Arrange()
        {
            _subject = new RegisterUserHandler(Context);

            Context.TenantInvites.Add(new TenantInvite
            {
                TenantId = _existingTenantId,
                Expiration = DateTime.UtcNow.AddHours(-1),
                Token = _inviteToken
            });

            Context.SaveChanges();
        }

        [Fact]
        public void exception_is_thrown()
        {
            Action t = () =>
            {
                _subject.Handle(new RegisterUser
                {
                    Email = _email,
                    Token = _inviteToken
                }, CancellationToken.None).GetAwaiter().GetResult();
            };

            t.Should().Throw<ArgumentException>();
        }
    }
}