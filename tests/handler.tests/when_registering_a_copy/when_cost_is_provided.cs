using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using Xunit;

namespace handler.tests.when_registering_a_copy
{
    public class when_cost_is_provided : InMemoryContext<GameTrackerContext>
    {
        private RegisterCopyHandler _subject;
        private Guid _gameId = new Guid("EA3B0EA5-D005-4D2F-95EF-9894132EC63E");
        private decimal _cost = 19.99m;

        public when_cost_is_provided()
        {
            _subject = new RegisterCopyHandler(Context);

            _subject.Handle(new RegisterCopy
            {
                GameId = _gameId,
                Cost = _cost
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void value_is_record()
        {
            var copy = Context.Copies.SingleOrDefault(c => c.GameId == _gameId);

            copy.Cost.Should().NotBeNull();
            copy.Cost.Should().Be(_cost);
        }
    }
}