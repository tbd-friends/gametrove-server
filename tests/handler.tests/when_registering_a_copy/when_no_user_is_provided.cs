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

namespace handler.tests.when_registering_a_copy
{
    public class when_no_user_is_provided : InMemoryContext<GameTrackerContext>
    {
        private readonly RegisterCopyHandler _subject;
        private Mock<IMediator> _mediator;
        private Guid _gameId = new Guid("EA3B0EA5-D005-4D2F-95EF-9894132EC63E");
        private decimal _cost = 19.99m;

        public when_no_user_is_provided()
        {
            _mediator = new Mock<IMediator>();
            _subject = new RegisterCopyHandler(Context, _mediator.Object);

            _subject.Handle(new RegisterCopy
            {
                GameId = _gameId,
                Cost = _cost
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void copy_is_not_registered()
        {
            Context.Copies.Count().Should().Be(0);
        }
    }
}