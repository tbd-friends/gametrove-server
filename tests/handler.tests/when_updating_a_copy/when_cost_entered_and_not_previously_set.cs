using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using Xunit;

namespace handler.tests.when_updating_a_copy
{
    public class when_cost_entered_and_not_previously_set : InMemoryContext<GameTrackerContext>
    {
        public UpdateCopyHandler _subject;

        private readonly Guid GameCopyId = new Guid("DDD61E0D-4E09-4077-A3A9-5E3257BCD413");
        private decimal GameCost = decimal.One;

        public when_cost_entered_and_not_previously_set()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new UpdateCopyHandler(Context);

            Context.Copies.Add(new Copy
            {
                Id = GameCopyId,
                Tags = null,
                GameId = new Guid("799C783F-7D02-4C1A-AE98-E713DCC6D138"),
                Cost = null,
                Purchased = null
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _subject.Handle(new UpdateCopy
            {
                Id = GameCopyId,
                Cost = GameCost
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void purchase_cost_is_updated()
        {
            Context.Copies.Single().Cost.Should().Be(GameCost);
        }
    }
}