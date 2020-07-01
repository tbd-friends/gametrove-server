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

namespace handler.tests.when_registering_user
{
    public class when_email_is_found : InMemoryContext<GameTrackerContext>
    {
        private RegisterUserHandler _subject;
        private readonly string _email = "EmailAddress";
        private readonly string _identifier = "Identifier";
        private RegisterUserResult _result;
        private readonly Guid _userId = new Guid("6E17E727-37A8-45FD-8F3F-4E5B7C6CEA9A");
        private readonly Guid _tenantId = new Guid("EC5B073D-F597-4205-98FE-2634875C565A");

        public when_email_is_found()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context.Users.Add(new User
            {
                Id = _userId,
                Email = _email,
                Identifier = _identifier,
                TenantId = _tenantId
            });

            Context.SaveChanges();

            _subject = new RegisterUserHandler(Context);
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterUser
            {
                Email = _email,
                Identifier = _identifier
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void user_id_is_returned()
        {
            _result.UserId.Should().Be(_userId);
            _result.TenantId.Should().NotBeEmpty();
        }

        [Fact]
        public void user_is_not_added()
        {
            Context.Users.Count(c => c.Email == _email).Should().Be(1);
        }
    }
}