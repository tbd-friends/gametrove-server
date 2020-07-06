using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.Infrastructure;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using Moq;
using Xunit;

namespace handler.tests.when_verifying_user
{
    public class when_user_exists : InMemoryContext<GameTrackerContext>
    {
        private VerifyUserHandler _subject;
        private Mock<IAuthenticatedMediator> _mediator;
        private bool _result;

        private string _email = "email@domain.com";

        public when_user_exists()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IAuthenticatedMediator>();

            Context.Users.Add(new User()
            {
                Email = _email
            });

            Context.SaveChanges();

            _subject = new VerifyUserHandler(Context, _mediator.Object);
        }

        private void Act()
        {
            _result = _subject.Handle(new VerifyUser
            {
                Email = _email
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void verify_returns_true()
        {
            _result.Should().Be(true);
        }
    }
}