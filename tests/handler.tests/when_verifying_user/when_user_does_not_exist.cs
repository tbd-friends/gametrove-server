using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using Moq;
using Xunit;

namespace handler.tests.when_verifying_user
{
    public class when_user_does_not_exist : InMemoryContext<GameTrackerContext>
    {
        private Mock<IAuthenticatedMediator> _mediator;
        private VerifyUserHandler _subject;
        private bool _result;

        private string _email = "email@domain.com";
        private readonly Guid _userId = new Guid("61D6517C-0736-429F-97E8-563A21F5C787");
        private readonly Guid _tenantId = new Guid("B9B21C29-0B29-4982-8F2B-9B704CF54A8A");

        public when_user_does_not_exist()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IAuthenticatedMediator>();

            _mediator.Setup(mediator =>
                    mediator.Send(
                        It.Is<RegisterUser>(command => command.Email == _email), CancellationToken.None))
                .Returns(Task.FromResult(new RegisterUserResult
                {
                    UserId = _userId,
                    TenantId = _tenantId
                }));

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
        public void register_user_is_called()
        {
            _mediator.Verify(mediator =>
                    mediator.Send(It.Is<RegisterUser>(command => command.Email == _email), CancellationToken.None),
                Times.Once);
        }

        [Fact]
        public void result_is_true()
        {
            _result.Should().BeTrue();
        }
    }
}