using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using Xunit;

namespace handler.tests.when_registering_user
{
    public class when_email_is_not_found : InMemoryContext<GameTrackerContext>
    {
        private RegisterUserHandler _subject;
        private string _email = "EmailAddress";
        private string _identifier = "Identifier";
        private RegisterUserResult _result;

        public when_email_is_not_found()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
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
        public void user_is_returned()
        {
            _result.UserId.Should().NotBeEmpty();
            _result.TenantId.Should().NotBeEmpty();
        }

        [Fact]
        public void user_is_added()
        {
            var user = Context.Users.SingleOrDefault(u => u.Email == _email);

            user.Should().NotBeNull();
            user.Email.Should().Be(_email);
            user.Identifier.Should().Be(_identifier);
        }
    }
}