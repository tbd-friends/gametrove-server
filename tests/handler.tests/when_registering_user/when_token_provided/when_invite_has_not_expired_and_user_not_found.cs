using System;
using System.Linq;
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
    public class when_invite_has_not_expired_and_user_not_found : InMemoryContext<GameTrackerContext>
    {
        private RegisterUserHandler _subject;
        private string _email = "EmailAddress";
        private string _identifier = "Identifier";
        private string _inviteToken = "Invitation";
        private readonly Guid _existingTenantId = new Guid("104128AB-68E9-4BF8-A223-5B04DC085292");
        private RegisterUserResult _result;

        public when_invite_has_not_expired_and_user_not_found()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new RegisterUserHandler(Context);

            Context.TenantInvites.Add(new TenantInvite
            {
                TenantId = _existingTenantId,
                Expiration = DateTime.UtcNow.AddHours(1),
                Token = _inviteToken
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterUser
            {
                Email = _email,
                Token = _inviteToken
            }, CancellationToken.None).GetAwaiter().GetResult();
        }
        [Fact]
        public void user_is_added()
        {
            var user = Context.Users.SingleOrDefault(u => u.Email == _email);

            user.Should().NotBeNull("User not found");
        }

        [Fact]
        public void user_is_assigned_to_existing_tenant()
        {
            var user = Context.Users.SingleOrDefault(u => u.Email == _email);

            user?.TenantId.Should().Be(_existingTenantId);
            _result.TenantId.Should().Be(_existingTenantId);
        }
    }
}