using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using MediatR;
using Moq;
using Xunit;

namespace handler.tests.when_adding_a_copy
{
    public class when_marked_as_is_wanted : InMemoryContext<GameTrackerContext>
    {
        private AddCopyHandler _addCopy;
        private Mock<IMediator> _mediator;

        private Guid GameId = new Guid("0617C481-165F-4629-99EC-DB6122056F19");

        public when_marked_as_is_wanted()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _addCopy = new AddCopyHandler(Context, _mediator.Object);
        }

        private void Act()
        {
            _addCopy.Handle(new AddCopy
            {
                GameId = GameId,
                IsWanted = true,
                Email = "Email",
                Identifier = "Identifier"
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void copy_is_marked_as_wanted()
        {
            Context.Copies.Single().IsWanted.Should().BeTrue();
        }
    }
}